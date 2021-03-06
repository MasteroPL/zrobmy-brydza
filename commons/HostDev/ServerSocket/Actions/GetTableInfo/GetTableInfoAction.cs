﻿using EasyHosting.Models.Actions;
using EasyHosting.Models.Serialization;
using EasyHosting.Models.Server;
using GameManagerLib.Models;
using ServerSocket.Models;
using ServerSocket.Serializers;
using System;

namespace ServerSocket.Actions.GetTableInfo {

    public class GetTableInfoAction : BaseAction {

        public GetTableInfoAction() : base(
            typeof(RequestSerializer),
            typeof(ResponseSerializer)
        ) { }

        protected override BaseSerializer PerformAction(ClientConnection conn, BaseSerializer requestData) {
            RequestSerializer data = (RequestSerializer)requestData;
            ResponseSerializer resp = (ResponseSerializer)InitializeResponseSerializer();

            Lobby lobby = (Lobby)conn.Session.Get("joined-lobby");
            Match game = lobby.Game;

            // Serializacja podstawowych informacji
            resp.Status = "OK";
            resp.GameState = (int)game.GameState;
            resp.Dealer = (int)game.Dealer;
            resp.PointsNSAboveLine = game.PointsNS[1];
            resp.PointsNSBelowLine = game.PointsNS[0];
            resp.PointsWEAboveLine = game.PointsWE[1];
            resp.PointsWEBelowLine = game.PointsWE[0];

            resp.NumberOfPlayers = game.PlayerList.Count;
            resp.Players = new PlayerSerializer[4];
            resp.NumberOfLobbyUsers = lobby.ConnectedClients.Count;
            resp.LobbyUsers = new LobbyUserSerializer[lobby.ConnectedClients.Count];
            for (int i = 0; i < 4; i++) resp.Players[i] = null;

            // Serializacja graczy przy stole
            PlayerSerializer tmpPlayer;
            int index = 0;
            foreach(var player in game.PlayerList) {
                tmpPlayer = new PlayerSerializer();

                tmpPlayer.PlayerTag = (int)player.Tag;
                tmpPlayer.Username = player.Name;

                resp.Players[index] = tmpPlayer;

                index++;
            }

            // Serializacja użytkowników w lobby
            LobbyUserSerializer tmpLobbyUser;
            Player tmpGamePlayer;
            index = 0;
            foreach(var lobbyUser in lobby.ConnectedClients) {
                tmpLobbyUser = new LobbyUserSerializer();

                tmpLobbyUser.Username = lobbyUser.Session.Get<string>("username");

                if(!lobbyUser.Session.Has("player")) {
                    tmpLobbyUser.IsSitted = false;
                    tmpLobbyUser.PlayerTag = -1;
                }
                else {
                    tmpGamePlayer = lobbyUser.Session.Get<Player>("player");
                    tmpLobbyUser.IsSitted = true;
                    tmpLobbyUser.PlayerTag = (int)tmpGamePlayer.Tag;
                }

                resp.LobbyUsers[index] = tmpLobbyUser;

                index++;
            }


            // test API for bidding serialization
            /*var match = new Match();
            match.AddPlayer(new Player(PlayerTag.W, "WPlayer"));
            match.AddPlayer(new Player(PlayerTag.S, "SPlayer"));
            match.AddPlayer(new Player(PlayerTag.E, "EPlayer"));
            match.AddPlayer(new Player(PlayerTag.N, "NPlayer"));

            match.Start();
            match.AddBid(new Contract(ContractHeight.ONE, ContractColor.C, PlayerTag.E));
            match.AddBid(new Contract(ContractHeight.ONE, ContractColor.D, PlayerTag.S));
            match.AddBid(new Contract(ContractHeight.ONE, ContractColor.H, PlayerTag.W));
            match.AddBid(new Contract(ContractHeight.NONE, ContractColor.NONE, PlayerTag.N));
            match.AddBid(new Contract(ContractHeight.TWO, ContractColor.C, PlayerTag.E));
            match.AddBid(new Contract(ContractHeight.NONE, ContractColor.NONE, PlayerTag.S, true));
            match.AddBid(new Contract(ContractHeight.NONE, ContractColor.NONE, PlayerTag.W, false, true));
            match.AddBid(new Contract(ContractHeight.NONE, ContractColor.NONE, PlayerTag.N));
            match.AddBid(new Contract(ContractHeight.NONE, ContractColor.NONE, PlayerTag.E));
            match.AddBid(new Contract(ContractHeight.NONE, ContractColor.NONE, PlayerTag.S));

            if (true)//game.GameState == GameState.BIDDING)
            {
                var tmp = new BiddingSerializer();
                tmp.CurrentPlayerTag = (int)match.CurrentBidding.CurrentPlayer;
                tmp.ContractList = new ContractSerializer[match.CurrentBidding.ContractList.Count];

                Contract tmpContract;
                for (int i = 0; i < match.CurrentBidding.ContractList.Count; i++)
                {
                    tmpContract = match.CurrentBidding.ContractList[i];
                    tmp.ContractList[i] = new ContractSerializer();

                    tmp.ContractList[i].ContractColor = (int)tmpContract.ContractColor;
                    tmp.ContractList[i].ContractHeight = (int)tmpContract.ContractHeight;
                    tmp.ContractList[i].XEnabled = tmpContract.XEnabled;
                    tmp.ContractList[i].XXEnabled = tmpContract.XXEnabled;
                    tmp.ContractList[i].PlayerTag = (int)tmpContract.DeclaredBy;
                }

                tmp.HighestContract = new ContractSerializer();
                tmp.HighestContract.ContractColor = (int)match.CurrentBidding.HighestContract.ContractColor;
                tmp.HighestContract.ContractHeight = (int)match.CurrentBidding.HighestContract.ContractHeight;
                tmp.HighestContract.XEnabled = match.CurrentBidding.HighestContract.XEnabled;
                tmp.HighestContract.XXEnabled = match.CurrentBidding.HighestContract.XXEnabled;
                tmp.HighestContract.PlayerTag = (int)match.CurrentBidding.HighestContract.DeclaredBy;

                tmp.Dealer = (int)PlayerTag.N;
                tmp.BiddingEnded = match.CurrentBidding.IsEnd();

                resp.CurrentBidding = tmp;
            }*/
            // end test for bidding serialization

            // Serializacja licytacji
            if (game.GameState == GameState.BIDDING) {
                var tmp = new BiddingSerializer();
                tmp.CurrentPlayerTag = (int)game.CurrentBidding.CurrentPlayer;
                tmp.ContractList = new ContractSerializer[game.CurrentBidding.ContractList.Count];

                Contract tmpContract;
                for(int i = 0; i < game.CurrentBidding.ContractList.Count; i++) {
                    tmpContract = game.CurrentBidding.ContractList[i];
                    tmp.ContractList[i] = new ContractSerializer();

                    tmp.ContractList[i].ContractColor = (int)tmpContract.ContractColor;
                    tmp.ContractList[i].ContractHeight = (int)tmpContract.ContractHeight;
                    tmp.ContractList[i].XEnabled = tmpContract.XEnabled;
                    tmp.ContractList[i].XXEnabled = tmpContract.XXEnabled;
                    tmp.ContractList[i].PlayerTag = (int)tmpContract.DeclaredBy;
                }

                tmp.HighestContract = new ContractSerializer();
                tmp.HighestContract.ContractColor = (int)game.CurrentBidding.HighestContract.ContractColor;
                tmp.HighestContract.ContractHeight = (int)game.CurrentBidding.HighestContract.ContractHeight;
                tmp.HighestContract.XEnabled = game.CurrentBidding.HighestContract.XEnabled;
                tmp.HighestContract.XXEnabled = game.CurrentBidding.HighestContract.XXEnabled;
                tmp.HighestContract.PlayerTag = (int)game.CurrentBidding.HighestContract.DeclaredBy;

                tmp.Dealer = (int)game.Dealer;
                tmp.BiddingEnded = game.CurrentBidding.IsEnd();

                resp.CurrentBidding = tmp;
            }
            else {
                resp.CurrentBidding = null;
            }

            // Rundy
            resp.RoundsNS = game.RoundsNS;
            resp.RoundsWE = game.RoundsWE;

            return resp;
        }
    }
}
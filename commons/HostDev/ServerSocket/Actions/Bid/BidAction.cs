using EasyHosting.Models.Actions;
using EasyHosting.Models.Serialization;
using EasyHosting.Models.Server;
using ServerSocket.Models;
using ServerSocket.Serializers;
using GameManagerLib.Exceptions;
using GameManagerLib.Models;
using System;

namespace ServerSocket.Actions.Bid
{
    public class BidAction : BaseAction {
        public BidAction() : base(
            typeof(RequestSerializer),
            typeof(ResponseSerializer)
        ) { }

        protected override BaseSerializer PerformAction(ClientConnection conn, BaseSerializer requestData) {
            RequestSerializer data = (RequestSerializer)requestData;
            ResponseSerializer resp = (ResponseSerializer)InitializeResponseSerializer();
            resp.Status = "OK";
            resp.Color = data.Color;
            resp.Height = data.Height;
            resp.X = data.X;
            resp.XX = data.XX;

            Lobby lobby = (Lobby)conn.Session.Get("joined-lobby");
            Match game = lobby.Game;

            if (lobby.LobbyState != LobbyState.IN_GAME) {
                data.AddError(null, "INVALID_LOBBY_STATE", "Nie można licytować w tym stanie lobby");
                data.ThrowException();
            }

            string username = (string)conn.Session.Get("username");

            int playerIndex = game.PlayerList.FindIndex(x => { return username == x.Name; });
            // gdy nie ma takiego gracza przy stole => wywal wyjątek
            if (playerIndex == -1)
            {
                data.AddError("PlayerUsername", "INVALID_PLAYER", "Nie ma takiego gracza");
                data.ThrowException();
            }

            var player = game.PlayerList[playerIndex];
            var contract = new Contract((ContractHeight)data.Height, (ContractColor)data.Color, player.Tag, data.X, data.XX);

            if (!game.CheckAddBid(contract)) {
                data.AddError(null, "INVALID_CONTRACT", "Nie można zalicytować kontraktu");
                data.ThrowException();
            }
            game.AddBid(contract);

            Console.WriteLine(username + "> Bid: " + contract.ToString());

            // Broadcast do pozostałych graczy
            var broadcastData = new LobbySignalNewBidSerializer() {
                Signal = LobbySignalNewBidSerializer.SIGNAL_NEW_BID,
                Username = username,
                PlaceTag = (int)player.Tag,
                Color = data.Color,
                Height = data.Height,
                X = data.X,
                XX = data.XX
            };
            var broadcastWrapper = new StandardCommunicateSerializer() {
                CommunicateType = StandardCommunicateSerializer.TYPE_LOBBY_SIGNAL,
                Data = broadcastData.GetApiObject()
            };
            lobby.Broadcast(broadcastWrapper.GetApiObject());

            // Broadcast o przepasowaniu całej licytacji, nowe rozdanie
            if(game.CurrentBidding.IsEnd() && game.CurrentBidding.HighestContract.ContractColor == ContractColor.NONE) {
                game.GameState = GameState.BIDDING;
                game.StartBidding();

                var bData = new LobbySignalGameStartedNextBiddingSerializer() {
                    Signal = LobbySignalGameStartedNextBiddingSerializer.SIGNAL_GAME_STARTED_NEXT_BIDDING,

                    PointsNSAboveLine = game.PointsNS[1],
                    PointsNSBelowLine = game.PointsNS[0],
                    PointsWEAboveLine = game.PointsWE[1],
                    PointsWEBelowLine = game.PointsWE[0],

                    RoundsNS = game.RoundsNS,
                    RoundsWE = game.RoundsWE
                };
                var bWrapper = new StandardCommunicateSerializer() {
                    CommunicateType = StandardCommunicateSerializer.TYPE_LOBBY_SIGNAL,
                    Data = bData.GetApiObject()
                };
                lobby.Broadcast(bWrapper.GetApiObject());
            }

            return resp;
        }
    }
}

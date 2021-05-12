﻿using EasyHosting.Models;
using EasyHosting.Models.Actions;
using EasyHosting.Models.Client;
using GameManagerLib.Models;
using Newtonsoft.Json.Linq;
using ServerSocket.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetTableInfoActionRequestSerializer = ServerSocket.Actions.GetTableInfo.RequestSerializer;
using GetTableInfoActionResponseSerializer = ServerSocket.Actions.GetTableInfo.ResponseSerializer;
using GetHandActionRequestSerializer = ServerSocket.Actions.GetHand.RequestSerializer;
using GetHandActionResponseSerializer = ServerSocket.Actions.GetHand.ResponseSerializer;
using SitActionRequestSerializer = ServerSocket.Actions.Sit.RequestSerializer;
using SitActionResponseSerializer = ServerSocket.Actions.Sit.ResponseSerializer;
using BidActionRequestSerializer = ServerSocket.Actions.Bid.RequestSerializer;
using BidActionResponseSerializer = ServerSocket.Actions.Bid.ResponseSerializer;
using EasyHosting.Models.Client.Serializers;
using GameManagerLib.Exceptions;

namespace ClientSocketTesting {
    public class DumbAI {
        public ClientSocket ClientSocket = null;
        public Match Game = null;
        public string Username;
        public PlayerTag Position;
        public List<Card> MyCards = new List<Card>();

        public void Init() {
            ClientSocket.SignalReceived += OnServerSignalReceive;
        }
        ~DumbAI() {
            ClientSocket.SignalReceived -= OnServerSignalReceive;
        }

        public static ActionsSerializer WrapRequestData(string actionName, JObject data) {
            var result = new ActionsSerializer();
            result.Actions = new ActionSerializer[1];
            var tmp = new ActionSerializer();

            tmp.ActionName = actionName;
            tmp.ActionData = data;

            result.Actions[0] = tmp;

            return result;
        }

        public void PerformServerAction(string actionName, JObject data, Action<Request, ActionsSerializer, object> callback = null, object additionalData = null) {
            var requestData = WrapRequestData(actionName, data);
            var currentRequest = ClientSocket.SendRequest(requestData.GetApiObject());

            while(currentRequest.RequestState != RequestState.RESPONSE_RECEIVED) {
                ClientSocket.UpdateCommunication();
            }

            var actsSer = new ActionsSerializer(currentRequest.ResponseData);
            actsSer.Validate();

            callback?.Invoke(currentRequest, actsSer, additionalData);
        }
        private void HandleLobbySignal(JObject signalData) {
            var signalName = (string)signalData.GetValue("signal");
            // start gry
            if (signalName == PlayerClickedGameStartSerializer.SIGNAL_PLAYER_READY) {
                var serializer = new PlayerClickedGameStartSerializer(signalData);
                serializer.Validate();

                if (serializer.Username != Username) {
                    Game.Start();
                }
            }
            else if (signalName == LobbySignalNewBidSerializer.SIGNAL_NEW_BID) {
                var serializer = new LobbySignalNewBidSerializer(signalData);
                serializer.Validate();

                try {
                    var contract = new Contract(
                        (ContractHeight)serializer.Height,
                        (ContractColor)serializer.Color,
                        (PlayerTag)serializer.PlaceTag,
                        serializer.X,
                        serializer.XX
                    );
                    Game.AddBid(contract);
                } catch (WrongBidException) {
                    // TODO: Pobrać całą licytację jeszcze raz
                }
            }
            // Użytkownik usiadł na wybranym miejscu
            else if (signalName == LobbySignalUserSatSerializer.SIGNAL_USER_SAT) {
                var serializer = new LobbySignalUserSatSerializer(signalData);
                serializer.Validate();

                if (serializer.Username != Username) {
                    Game.AddPlayer(new Player((PlayerTag)serializer.PlaceTag, serializer.Username));
                }
            }
        }
        private void OnServerSignalReceive(object sender, StandardResponseWrapperSerializer data) {
            switch (data.CommunicateType) {
                case "LOBBY_SIGNAL":
                    HandleLobbySignal(data.Data);
                    break;

                default:
                    //Debug.Log("Unrecognized signal");
                    break;
            }
        }

        public void Authorize() {
            var authData = new AuthorizationSerializer() {
                Login = Username,
                LobbyPassword = "",
                LobbyId = "DEFAULT"
            };
            var authRequest = ClientSocket.SendRequest(authData.GetApiObject());
            while(authRequest.RequestState != RequestState.RESPONSE_RECEIVED) {
                ClientSocket.UpdateCommunication();
            }
            Console.WriteLine("Authorized " + Username);
        }

        public void LoadGame() {
            PerformServerAction("get-table-info", null, LoadGameCallback, null);
        }
        protected void LoadGameCallback(Request request, ActionsSerializer response, object additionalData) {
            Game = new Match();
            MyCards.Clear();

            var rs = new GetTableInfoActionResponseSerializer(response.Actions[0].ActionData);
            rs.Validate();

            Game.RoundsNS = rs.RoundsNS;
            Game.RoundsWE = rs.RoundsWE;
            Game.PointsNS[0] = rs.PointsNSBelowLine;
            Game.PointsNS[1] = rs.PointsNSAboveLine;
            Game.PointsWE[0] = rs.PointsWEBelowLine;
            Game.PointsWE[1] = rs.PointsWEAboveLine;

            for (int i = 0; i < 4; i++) {
                if (rs.Players[i] != null) {
                    Game.AddPlayer(new Player((PlayerTag)rs.Players[i].PlayerTag, rs.Players[i].Username));
                }
            }

            if (rs.CurrentBidding != null && rs.CurrentBidding.ContractList != null) {
                foreach(var contract in rs.CurrentBidding.ContractList) {
                    if(contract != null) {
                        Game.AddBid(new Contract(
                            (ContractHeight)contract.ContractHeight,
                            (ContractColor)contract.ContractColor,
                            (PlayerTag)contract.PlayerTag,
                            contract.XEnabled,
                            contract.XXEnabled
                        ));
                    }
                }
            }
        }

        public void Sit() {
            var sitRequestData = new SitActionRequestSerializer() {
                PlaceTag = (int)Position
            };
            PerformServerAction("sit", sitRequestData.GetApiObject(), null, null);
            Game.AddPlayer(new Player(Position, Username));
        }


        public void GetHand() {
            var getHandRequestData = new GetHandActionRequestSerializer() {
                PlayerID = (int)Position,
                Username = Username
            };
            PerformServerAction("get-hand", getHandRequestData.GetApiObject(), GetHandCallback);
        }
        protected void GetHandCallback(Request request, ActionsSerializer response, object additionalData) {
            var data = new GetHandActionResponseSerializer(response.Actions[0].ActionData);
            data.Validate();

            foreach(var card in data.Cards) {
                MyCards.Add(new Card(
                    (CardFigure)card.Figure,
                    (CardColor)card.Color,
                    Position,
                    (CardState)card.State
                ));
            }
        }

        public void Play() {
            ClientSocket.UpdateCommunication();
            switch (Game.GameState){
                case GameState.BIDDING:
                    PlayBidding();
                    break;
                case GameState.PLAYING:
                    PlayPlaying();
                    break;
            }
        }
        protected virtual void PlayBidding() {
            if(Game.CurrentBidding.CurrentPlayer == Position) {
                var data = new BidActionRequestSerializer() {
                    Height = (int)ContractHeight.NONE,
                    Color = (int)ContractColor.NONE,
                    X = false,
                    XX = false
                };

                Console.WriteLine(Username + "> Licytuje: pas");
                PerformServerAction("bid", data.GetApiObject(), null, null);
            }
        }
        protected virtual void PlayPlaying() {
            if(Game.CurrentGame.CurrentPlayer == Position) {
                for(int i = 0; i < MyCards.Count; i++) {
                    if (Game.CurrentGame.CheckNextCard(MyCards[i])) {


                        //PerformServerAction("put-card")
                    }
                }
            }
        }
    }
}

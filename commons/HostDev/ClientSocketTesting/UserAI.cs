using EasyHosting.Models;
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
using PutCardActionRequestSerializer = ServerSocket.Actions.PutCard.RequestSerializer;
using PutCardActionResponseSerializer = ServerSocket.Actions.PutCard.ResponseSerializer;
using EasyHosting.Models.Client.Serializers;
using GameManagerLib.Exceptions;
using StartGameActionRequestSerializer = ServerSocket.Actions.StartGame.RequestSerializer;

namespace ClientSocketTesting
{
    public class UserAI
    {
        public ClientSocket ClientSocket = null;
        public Match Game = null;
        public string Username;
        public PlayerTag Position;
        public bool SendGetHandRequest = false;
        public AI AILogic = null;
        bool playing = false;


        public void Init()
        {
            ClientSocket.SignalReceived += OnServerSignalReceive;
        }
        ~UserAI()
        {
            ClientSocket.SignalReceived -= OnServerSignalReceive;
        }

        public static ActionsSerializer WrapRequestData(string actionName, JObject data)
        {
            var result = new ActionsSerializer();
            result.Actions = new ActionSerializer[1];
            var tmp = new ActionSerializer();

            tmp.ActionName = actionName;
            tmp.ActionData = data;

            result.Actions[0] = tmp;

            return result;
        }

        public void PerformServerAction(string actionName, JObject data, Action<Request, ActionsSerializer, object> callback = null, object additionalData = null)
        {
            var requestData = WrapRequestData(actionName, data);
            var currentRequest = ClientSocket.SendRequest(requestData.GetApiObject());

            while (currentRequest.RequestState != RequestState.RESPONSE_RECEIVED)
            {
                ClientSocket.UpdateCommunication();
            }

            var actsSer = new ActionsSerializer(currentRequest.ResponseData);
            actsSer.Validate();

            callback?.Invoke(currentRequest, actsSer, additionalData);
        }
        private void HandleLobbySignal(JObject signalData)
        {
            var signalName = (string)signalData.GetValue("signal");
            // start gry
            if (signalName == PlayerClickedGameStartSerializer.SIGNAL_PLAYER_READY)
            {
                var serializer = new PlayerClickedGameStartSerializer(signalData);
                serializer.Validate();

                Game.Start();
                SendGetHandRequest = true;
            }
            else if (signalName == LobbySignalNewBidSerializer.SIGNAL_NEW_BID)
            {
                var serializer = new LobbySignalNewBidSerializer(signalData);
                serializer.Validate();

                try
                {
                    var contract = new Contract(
                        (ContractHeight)serializer.Height,
                        (ContractColor)serializer.Color,
                        (PlayerTag)serializer.PlaceTag,
                        serializer.X,
                        serializer.XX
                    );
                    Game.AddBid(contract);
                }
                catch (WrongBidException)
                {
                    // TODO: Pobrać całą licytację jeszcze raz
                }
            }
            // Użytkownik usiadł na wybranym miejscu
            else if (signalName == LobbySignalUserSatSerializer.SIGNAL_USER_SAT)
            {
                var serializer = new LobbySignalUserSatSerializer(signalData);
                serializer.Validate();

                if (serializer.Username != Username)
                {
                    Game.AddPlayer(new Player((PlayerTag)serializer.PlaceTag, serializer.Username));
                }
            }
            else if (signalName == LobbySignalUserSittedOutSerializer.SIGNAL_USER_SITTED_OUT)
            {
                var serializer = new LobbySignalUserSittedOutSerializer(signalData);
                serializer.Validate();

                var player = Game.GetPlayerAt((PlayerTag)serializer.PlaceTag);
                Game.RemovePlayer(player);
            }
            else if (signalName == PutCardSignalSerializer.SIGNAL_USER_PUT_CARD)
            {
                var serializer = new PutCardSignalSerializer(signalData);
                serializer.Validate();

                if (serializer.Username != Username)
                {
                    Game.CurrentGame.NextCard(new Card((CardFigure)serializer.CardFigure, (CardColor)serializer.CardColor, (PlayerTag)serializer.OwnerPosition));
                }
            }
            // Następna licytacja
            else if (signalName == LobbySignalGameStartedNextBiddingSerializer.SIGNAL_GAME_STARTED_NEXT_BIDDING)
            {
                var serializer = new LobbySignalGameStartedNextBiddingSerializer(signalData);

                Game.ClearPlayerHands();

                AILogic = null;

                if (Game.GameState != GameState.BIDDING)
                {
                    Game.GameState = GameState.BIDDING;
                    Game.StartBidding();
                }
            }
            else if (signalName == LobbySignalGameFinishedSerializer.SIGNAL_GAME_FINISHED)
            {
                var serializer = new LobbySignalGameFinishedSerializer(signalData);

                Game.ClearPlayerHands();
  
                Game.GameState = GameState.GAME_FINISHED;
            }
        }
        private void OnServerSignalReceive(object sender, StandardResponseWrapperSerializer data)
        {
            switch (data.CommunicateType)
            {
                case "LOBBY_SIGNAL":
                    HandleLobbySignal(data.Data);
                    break;

                default:
                    //Debug.Log("Unrecognized signal");
                    break;
            }
        }

        public void Authorize()
        {
            var authData = new AuthorizationSerializer()
            {
                Login = Username,
                LobbyPassword = "",
                LobbyId = "DEFAULT"
            };
            var authRequest = ClientSocket.SendRequest(authData.GetApiObject());
            while (authRequest.RequestState != RequestState.RESPONSE_RECEIVED)
            {
                ClientSocket.UpdateCommunication();
            }
            Console.WriteLine("Authorized " + Username);
        }

        public void LoadGame()
        {
            PerformServerAction("get-table-info", null, LoadGameCallback, null);
        }
        protected void LoadGameCallback(Request request, ActionsSerializer response, object additionalData)
        {
            Game = new Match(enableCardsShufflingAndDistributing: false);

            AILogic = null;

            var rs = new GetTableInfoActionResponseSerializer(response.Actions[0].ActionData);
            rs.Validate();

            Game.RoundsNS = rs.RoundsNS;
            Game.RoundsWE = rs.RoundsWE;
            Game.PointsNS[0] = rs.PointsNSBelowLine;
            Game.PointsNS[1] = rs.PointsNSAboveLine;
            Game.PointsWE[0] = rs.PointsWEBelowLine;
            Game.PointsWE[1] = rs.PointsWEAboveLine;

            for (int i = 0; i < 4; i++)
            {
                if (rs.Players[i] != null)
                {
                    Game.AddPlayer(new Player((PlayerTag)rs.Players[i].PlayerTag, rs.Players[i].Username));
                }
            }

            if (rs.CurrentBidding != null && rs.CurrentBidding.ContractList != null)
            {
                foreach (var contract in rs.CurrentBidding.ContractList)
                {
                    if (contract != null)
                    {
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

        public void Sit()
        {
            var sitRequestData = new SitActionRequestSerializer()
            {
                PlaceTag = (int)Position
            };
            PerformServerAction("sit", sitRequestData.GetApiObject(), null, null);
            Game.AddPlayer(new Player(Position, Username));
        }


        public void GetHand()
        {
            var getHandRequestData = new GetHandActionRequestSerializer()
            {
                PlayerTag = (int)Position
            };
            PerformServerAction("get-hand", getHandRequestData.GetApiObject(), GetHandCallback);
        }
        protected void GetHandCallback(Request request, ActionsSerializer response, object additionalData)
        {
            SendGetHandRequest = false;
            var data = new GetHandActionResponseSerializer(response.Actions[0].ActionData);
            data.Validate();

            int index = 0;
            Card tmp;
            var player = Game.GetPlayerAt(Position);

            List<int> C = new List<int>();
            List<int> D = new List<int>();
            List<int> H = new List<int>();
            List<int> S = new List<int>();

            foreach (var card in data.Cards)
            {
                tmp = new Card(
                    (CardFigure)card.Figure,
                    (CardColor)card.Color,
                    Position,
                    (CardState)card.State
                );
                
                player.Hand[index] = tmp;
                index++;

                if(card.Color + 1 == 1)
                {
                    C.Add(card.Figure);
                }

                if (card.Color + 1 == 2)
                {
                    D.Add(card.Figure);
                }

                if (card.Color + 1 == 3)
                {
                    H.Add(card.Figure);
                }

                if (card.Color + 1 == 4)
                {
                    S.Add(card.Figure);
                }
                
                AILogic = new AI(C, D, H, S);
            }
            //printy
            Console.Write("C: ");
            foreach (int c in C)
            {
                Console.Write(c);
                Console.Write(" ");
            }

            Console.Write("D: ");
            foreach (int d in D)
            {
                Console.Write(d);
                Console.Write(" ");
            }

            Console.Write("H: ");
            foreach (int h in H)
            {
                Console.Write(h);
                Console.Write(" ");
            }

            Console.Write("S: ");
            foreach (int s in S)
            {
                Console.Write(s);
                Console.Write(" ");
            }
            //koniec
        }

        public void GetGrandpaHand()
        {
            var getHandRequestData = new GetHandActionRequestSerializer()
            {
                PlayerTag = ((int)Position + 2) % 4
            };
            PerformServerAction("get-hand", getHandRequestData.GetApiObject(), GetGrandpaHandCallback);
        }
        protected void GetGrandpaHandCallback(Request request, ActionsSerializer response, object additionalData)
        {
            var data = new GetHandActionResponseSerializer(response.Actions[0].ActionData);
            data.Validate();
           

            int index = 0;
            Card tmp;
            var player = Game.GetPlayerAt((PlayerTag)(((int)Position + 2) % 4));

            List<int> C = new List<int>();
            List<int> D = new List<int>();
            List<int> H = new List<int>();
            List<int> S = new List<int>();

            foreach (var card in data.Cards)
            {
                tmp = new Card(
                    (CardFigure)card.Figure,
                    (CardColor)card.Color,
                    (PlayerTag)(((int)Position + 2) % 4),
                    (CardState)card.State
                );
                if (card.Color + 1 == 1)
                {
                    C.Add(card.Figure);
                }

                if (card.Color + 1 == 2)
                {
                    D.Add(card.Figure);
                }

                if (card.Color + 1 == 3)
                {
                    H.Add(card.Figure);
                }

                if (card.Color + 1 == 4)
                {
                    S.Add(card.Figure);
                }
                player.Hand[index] = tmp;
                index++;
            }

            AILogic.Grandpa_hand = new AI.Hand(C, D, H, S);
        }


        public virtual void Play()
        {
            ClientSocket.UpdateCommunication();
            
            if (Game.GameState == GameState.STARTING)
            {
                var data = new StartGameActionRequestSerializer()
                {
                    Username = Username,
                    PlaceTag = (int)Position
                };
                PerformServerAction("start-game", data.GetApiObject(), null, null);
            }

            if (SendGetHandRequest)
            {
                GetHand();
            }
            switch (Game.GameState)
            {
                case GameState.BIDDING:
                    playing = false;
                    PlayBidding();
                    break;
                case GameState.PLAYING:
                    if (playing == false)
                    {
                        playing = true;
                        bool defense = true;
                        if((int)Game.CurrentGame.Declarer == (int)Position || (int)Game.CurrentGame.Declarer == ((int)Position + 2) %4)
                        {
                            defense = false;
                        }
                        AILogic.SetColorPriorityList((int)Game.CurrentGame.ContractColor + 1, defense);
                    }
                    PlayPlaying();
                    break;
            }
        }
        protected virtual void PlayBidding()
        {
            if (Game.CurrentBidding.CurrentPlayer == Position)
            {
                
                var history = Game.CurrentBidding.ContractList;

                List<int> AI_history = new List<int>();

                foreach(var bid in history)
                {
                    int AI_bid = ((int)(bid.ContractColor) + 1) + (int)(bid.ContractHeight) * 10;
                    AI_history.Add(AI_bid);
                }

                int newBid = AILogic.Bid(AI_history);
                int h;
                int c;

                if (newBid == 0)
                {
                    h = -1;
                    c = -1;
                }
                else
                {
                    h = newBid / 10;
                    c = newBid % 10 - 1;
                }
                var data = new BidActionRequestSerializer()
                {
                    Height = h,
                    Color = c,
                    X = false,
                    XX = false
                };

                Console.WriteLine(Username + "> Licytuje: pas");
                PerformServerAction("bid", data.GetApiObject(), null, null);
            }
        }
        protected virtual void PlayPlaying()
        {
            Console.WriteLine(Position);
            Console.WriteLine(Game.CurrentGame.CurrentPlayer);
            if (Game.CurrentBidding.Declarer == Position && AILogic.Grandpa_hand == null)
            {
                GetGrandpaHand();
            }

            if (
                ((int)Game.CurrentBidding.Declarer + 2) % 4 != (int)Position
                && Game.CurrentGame.CurrentPlayer == Position)
            {
                Console.WriteLine(Position);
                var trick = Game.CurrentGame.currentTrick.CardList;
                List<int> newTrick = new List<int>();
                foreach (var t in trick)
                {
                    newTrick.Add((int)t.Color + 1 + (int)t.Figure * 10);
                }
                
                int card = AILogic.PutCard(newTrick, (int)Game.CurrentGame.ContractColor + 1, AILogic.AI_hand);
   

                var data = new PutCardActionRequestSerializer()
                {
                    CardOwnerPosition = (int)Position,
                    Color = card % 10 - 1,
                    Figure = card / 10
                };
                Card NormalCard = new Card((CardFigure)data.Figure, (CardColor)data.Color, Position);
                PerformServerAction("put-card", data.GetApiObject(), PutCardCallback, NormalCard);

            }
            else if (
                Game.CurrentBidding.Declarer == Position
                && ((int)Game.CurrentGame.CurrentPlayer + 2) % 4 == (int)Position)
            {

                var trick = Game.CurrentGame.currentTrick.CardList;
                List<int> newTrick = new List<int>();
                foreach (var t in trick)
                {
                    newTrick.Add((int)t.Color + 1 + (int)t.Figure * 10);
                }

                int card = AILogic.PutCard(newTrick, (int)Game.CurrentGame.ContractColor + 1, AILogic.Grandpa_hand);

                var data = new PutCardActionRequestSerializer()
                {
                    CardOwnerPosition = ((int)Position + 2) % 4,
                    Color = card % 10 - 1,
                    Figure = card / 10
                };
                Card NormalCard = new Card((CardFigure)data.Figure, (CardColor)data.Color, (PlayerTag) (((int)Position + 2) % 4 ));
                PerformServerAction("put-card", data.GetApiObject(), PutCardCallback, NormalCard);
            }
        }
        protected void PutCardCallback(Request request, ActionsSerializer response, object additionalData)
        {
            var card = (Card)additionalData;
            Game.CurrentGame.NextCard(card);
        }
    }
}

using EasyHosting.Models.Actions;
using EasyHosting.Models.Serialization;
using EasyHosting.Models.Server;
using GameManagerLib.Models;
using ServerSocket.Models;
using GameManagerLib.Exceptions;
using ServerSocket.Serializers;
using System;

namespace ServerSocket.Actions.StartGame {
    public class StartGameAction : BaseAction {

        public StartGameAction() : base(
            typeof(RequestSerializer),
            typeof(ResponseSerializer)
        ) { }

        protected override BaseSerializer PerformAction(ClientConnection conn, BaseSerializer requestData) {
            RequestSerializer data = (RequestSerializer)requestData;
            ResponseSerializer resp = (ResponseSerializer)InitializeResponseSerializer();
            resp.Status = "OK";

            Lobby lobby = (Lobby)conn.Session.Get("joined-lobby");
            Match game = lobby.Game;

            try
            {
                game.Start();

                Console.WriteLine();
                var broadcastData = new PlayerClickedGameStartSerializer()
                {
                    Signal = PlayerClickedGameStartSerializer.SIGNAL_PLAYER_READY,
                    PlaceTag = data.PlaceTag,
                    Username = data.Username
                };

                var broadcastWrapper = new StandardCommunicateSerializer()
                {
                    CommunicateType = StandardCommunicateSerializer.TYPE_LOBBY_SIGNAL,
                    Data = broadcastData.GetApiObject()
                };
                lobby.Broadcast(broadcastWrapper.GetApiObject());
            }
            catch(WrongGameStateException e)
            {
                data.AddError(null, "WRONG_GAME_STATE", "Nieprawidłowy stan gry dla tej operacji");
                data.ThrowException();
            }
            catch(WrongPlayerException e)
            {
                data.AddError("PlaceTag", "WRONG_PLAYER_ID", "Nieprawidłowa pozycja gracza");
                data.ThrowException();
            }
            catch(InvalidActionException e)
            {
                data.AddError(null, "INVALID_OPERATION", "Nieprawidłowa operacja - gracz już zgłosił gotowość do gry");
                data.ThrowException();
            }
            return resp;
        }
    }
}

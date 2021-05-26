using EasyHosting.Models.Actions;
using EasyHosting.Models.Serialization;
using EasyHosting.Models.Server;
using GameManagerLib.Models;
using ServerSocket.Models;
using GameManagerLib.Exceptions;
using System;
using ServerSocket.Serializers;

namespace ServerSocket.Actions.Sit {
    public class SitAction : BaseAction {

        public SitAction() : base(
            typeof(RequestSerializer),
            typeof(ResponseSerializer)
        ) { }

        protected override BaseSerializer PerformAction(ClientConnection conn, BaseSerializer requestData) {
            RequestSerializer data = (RequestSerializer)requestData;
            ResponseSerializer resp = (ResponseSerializer)InitializeResponseSerializer();
            resp.Status = "OK";

            Lobby lobby = (Lobby)conn.Session.Get("joined-lobby");
            Match game = lobby.Game;

            if(lobby.LobbyState != LobbyState.IDLE) {
                data.AddError(null, "INVALID_LOBBY_STATE", "Nie można usiąść w tym momencie gry");
                data.ThrowException();
            }

            try {
                var player = new Player((PlayerTag)data.PlaceTag, (string)conn.Session.Get("username"));
                game.AddPlayer(player);
                conn.Session.Set("player", player);
                Console.WriteLine("Player sat at " + data.PlaceTag + ": " + conn.Session.Get("username"));

                var broadcastData = new LobbySignalUserSatSerializer() {
                    Signal = LobbySignalUserSatSerializer.SIGNAL_USER_SAT,
                    PlaceTag = data.PlaceTag,
                    Username = player.Name
                };
                var broadcastWrapper = new StandardCommunicateSerializer() {
                    CommunicateType = StandardCommunicateSerializer.TYPE_LOBBY_SIGNAL,
                    Data = broadcastData.GetApiObject()
                };
                lobby.Broadcast(broadcastWrapper.GetApiObject());
            } catch(SeatTakenException) {
                data.AddError("PlaceTag", "SEAT_TAKEN", "Miejsce jest już zajęte");
                data.ThrowException();
            } catch(DuplicatedPlayerNameException) {
                data.AddError(null, "ALREADY_SEATED", "Użytkownik zajął już miejsce");
                data.ThrowException();
            } catch(WrongGameStateException) {
                data.AddError(null, "WRONG_GAME_STATE", "Nieprawidłowy stan gry dla tej operacji");
                data.ThrowException();
            }

            return resp;
        }
    }
}

using EasyHosting.Models.Actions;
using EasyHosting.Models.Serialization;
using EasyHosting.Models.Server;
using GameManagerLib.Models;
using ServerSocket.Models;
using ServerSocket.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Actions.SitPlayerOut {
    public class SitPlayerOutAction : BaseAction {
        public SitPlayerOutAction() : base(
            typeof(RequestSerializer),
            typeof(ResponseSerializer)
        ) { }

        protected override BaseSerializer PerformAction(ClientConnection conn, BaseSerializer requestData) {
            RequestSerializer data = (RequestSerializer)requestData;
            ResponseSerializer resp = (ResponseSerializer)InitializeResponseSerializer();
            resp.Status = "OK";

            Lobby lobby = conn.Session.Get<Lobby>("joined-lobby");
            Match game = lobby.Game;
            
            // Przypadek nieprawidłowego stanu gry
            if(game.GameState != GameState.AWAITING_PLAYERS && game.GameState != GameState.GAME_FINISHED) {
                data.AddError(null, "GAME_STATE_INVALID", "Nie można usunąć gracza, kiedy gra jest w aktualnym stanie");
                data.ThrowException();
            }

            // Check czy ktokolwiek w ogóle siedzi na tym miejscu
            var playerAtPlace = lobby.Game.GetPlayerAt((PlayerTag)data.PlaceTag);
            if (playerAtPlace == null) {
                data.AddError("PlaceTag", "NO_PLAYER_HERE", "Żaden gracz nie siedzi na tym miejscu");
                data.ThrowException();
            }

            // Check uprawnień w przypadku, gdy nie jesteśmy właścicielem Lobby
            if (lobby.LobbyOwner != conn) {
                if(playerAtPlace.Name != conn.Session.Get<string>("username")) {
                    data.AddError(null, "FORBIDDEN", "Brak uprawnień do tej czynności.");
                    data.ThrowException();
                }
            }

            // Lepiej jeśli cokolwiek zepsuje się po usunięciu gracza ze stołu. Wtedy rozgrywkę będzie można kontynuować mimo błędu
            lobby.Game.RemovePlayer(playerAtPlace);

            var username = playerAtPlace.Name;
            var signal = new LobbySignalUserSittedOutSerializer() {
                Signal = LobbySignalUserSittedOutSerializer.SIGNAL_USER_SITTED_OUT,
                Message = "User " + username + " joined the lobby",
                Username = username,
                PlaceTag = (int)playerAtPlace.Tag
            };
            var result = new StandardCommunicateSerializer() {
                CommunicateType = StandardCommunicateSerializer.TYPE_LOBBY_SIGNAL,
                Data = signal.GetApiObject()
            };
            lobby.Broadcast(result.GetApiObject());

            var index = lobby.ConnectedClients.FindIndex((client) => {
                return client.Session.Get<string>("username") == playerAtPlace.Name;
            });

            if(index != -1) {
                var client = lobby.ConnectedClients[index];
                client.Session.Remove("player");
            }

            return resp;
        }
    }
}

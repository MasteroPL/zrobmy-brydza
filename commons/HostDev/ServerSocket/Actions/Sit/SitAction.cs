using EasyHosting.Models.Actions;
using EasyHosting.Models.Serialization;
using EasyHosting.Models.Server;
using GameManagerLib.Models;
using ServerSocket.Models;
using GameManagerLib.Exceptions;

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

            try {
                var player = new Player((PlayerTag)data.PlaceTag, (string)conn.Session.Get("username"));
                game.AddPlayer(player);
                conn.Session.Set("player", player);
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

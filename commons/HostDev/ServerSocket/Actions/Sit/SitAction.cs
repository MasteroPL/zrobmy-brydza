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
                game.AddPlayer(new Player((PlayerTag)data.PlaceTag, (string)conn.Session.Get("username")));
            } catch(SeatTakenException e) {
                data.AddError("PlaceTag", "SEAT_TAKEN", "Miejsce jest już zajęte");
                data.ThrowException();
            } catch(DuplicatedPlayerNameException e) {
                data.AddError(null, "ALREADY_SEATED", "Użytkownik zajął już miejsce");
                data.ThrowException();
            }

            return resp;
        }
    }
}

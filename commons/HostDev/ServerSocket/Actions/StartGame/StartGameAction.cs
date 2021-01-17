using EasyHosting.Models.Actions;
using EasyHosting.Models.Serialization;
using EasyHosting.Models.Server;
using GameManagerLib.Models;
using ServerSocket.Models;
using GameManagerLib.Exceptions;

namespace ServerSocket.Actions.StartGame {
    public class StartGameAction : BaseAction {

        public StartGameAction() : base(
            typeof(RequestSerializer),
            typeof(ResponseSerializer)
        ) { }

        protected override BaseSerializer PerformAction(ClientConnection conn, BaseSerializer requestData) {
            RequestSerializer data = (RequestSerializer)requestData;
            ResponseSerializer resp = (ResponseSerializer)InitializeResponseSerializer();
            resp.Status = 200;

            Lobby lobby = (Lobby)conn.Session.Get("joined-lobby");
            Match game = lobby.Game;

            try {
                game.AddPlayer(new Player((PlayerTag)data.PlaceTag, (string)conn.Session.Get("username")));
            } catch(SeatTakenException e) {
                resp.Status = 400;
                resp.Error = "To miejsce jest zajęte";
            } catch(DuplicatedPlayerNameException e) {
                resp.Status = 400;
                resp.Error = "Niepoprawna operacja";
            }

            return resp;
        }
    }
}

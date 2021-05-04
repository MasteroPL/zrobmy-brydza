using EasyHosting.Models.Actions;
using EasyHosting.Models.Serialization;
using EasyHosting.Models.Server;
using GameManagerLib.Models;
using ServerSocket.Models;
using GameManagerLib.Exceptions;
using System;

namespace ServerSocket.Actions.LeavePlace{

    public class LeavePlaceAction : BaseAction{

        public LeavePlaceAction() : base(
            typeof(RequestSerializer),
            typeof(ResponseSerializer)
        ) { }

        protected override BaseSerializer PerformAction(ClientConnection conn, BaseSerializer requestData){
            RequestSerializer data = (RequestSerializer)requestData;
            ResponseSerializer resp = (ResponseSerializer)InitializeResponseSerializer();
            resp.Status = "OK";

            Lobby lobby = (Lobby)conn.Session.Get("joined-lobby");
            Match game = lobby.Game;
            Player player = new Player((PlayerTag)data.PlaceTag, (string)conn.Session.Get("username"));

            if (player != null)
            {
                try
                {                    
                    game.RemovePlayer(player);
                    conn.Session.Remove("player");
                }
                catch (Exception e) {
                    //TO-DO nie wiem jakie może to wyrzucić wyjątki
                    // To się dowiedz
                }

            }
            return resp;
        }


    }
}

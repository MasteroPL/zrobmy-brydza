using EasyHosting.Models.Actions;
using EasyHosting.Models.Server;
using Newtonsoft.Json.Linq;
using ServerSocket.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Models {
    public class BridgeServerSocket : EasyHosting.Models.Server.ServerSocket {
        public LobbiesManager LobbiesManager = new LobbiesManager();

        protected override bool AuthorizeConnection(ClientConnection conn, JObject requestData) {
            var serializer = new AuthorizationSerializer(requestData);

            try {
                serializer.Validate();

                conn.Session.Set("username", serializer.Login);

                LobbiesManager.JoinLobby(conn, serializer.LobbyId, serializer.LobbyPassword);
            } catch(Exception) {
                return false;
            }

            return true;
        }

        protected override JObject HandleRequest(ClientConnection conn, JObject requestData) {
            var lobby = (Lobby)conn.Session.Get("joined-lobby");
            var response = lobby.ActionsManager.PerformActions(requestData);

            return response;
        }
    }
}

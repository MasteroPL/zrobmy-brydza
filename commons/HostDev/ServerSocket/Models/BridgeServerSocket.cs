using EasyHosting.Meta.Validators;
using EasyHosting.Models.Actions;
using EasyHosting.Models.Server;
using EasyHosting.Models.Server.Serializers;
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

        protected override JObject GetAuthorizationResponseSuccessful() {
            var result = new StandardCommunicateSerializer();
            result.CommunicateType = StandardCommunicateSerializer.TYPE_AUTHORIZATION;

            var response = new StandardResponseSerializer();
            response.Status = "OK";
            response.Message = "Authorization successful";
            result.Data = response.GetApiObject();

            return result.GetApiObject();
        }
        protected override JObject GetAuthorizationResponseFailed() {
            var result = new StandardCommunicateSerializer();
            result.CommunicateType = StandardCommunicateSerializer.TYPE_AUTHORIZATION;

            var response = new StandardResponseSerializer();
            response.Status = "FORBIDDEN";
            response.Message = "Authorization failed";
            result.Data = response.GetApiObject();

            return result.GetApiObject();
        }

        protected override bool AuthorizeConnection(ClientConnection conn, JObject requestData) {
            var serializer = new AuthorizationSerializer(requestData);

            try {
                serializer.Validate();

                LobbiesManager.JoinLobby(conn, serializer.LobbyId, serializer.LobbyPassword);
                conn.Session.Set("username", serializer.Login);
            } catch(Exception) {
                return false;
            }

            return true;
        }

        protected override JObject HandleRequest(ClientConnection conn, JObject requestData) {
            // Inicjalna walidacja (Bierzemy kod zapytania, jeśli został podany)
            var initialCheck = new StandardRequestSerializer(requestData);
            try {
                initialCheck.Validate();
            } catch (ValidationException) {
                // Inicjalny format nie przechodzi sprawdzenia
                var result = new StandardCommunicateSerializer();
                var response = new StandardResponseSerializer();

                result.CommunicateType = StandardCommunicateSerializer.TYPE_REQUEST_ERROR;
                response.Status = "INVALID_FORMAT";
                response.Message = "Request failed initial validation";
                result.Data = response.GetApiObject();

                return result.GetApiObject();
            }

            // Przekazanie danych do menadżera akcji
            try {
                var lobby = (Lobby)conn.Session.Get("joined-lobby");
                var response = lobby.ActionsManager.PerformActions(conn, requestData);

                var result = new StandardCommunicateSerializer();
                result.CommunicateType = StandardCommunicateSerializer.TYPE_RESPONSE;
                result.Data = response;
                result.RequestCode = initialCheck.RequestCode;

                return result.GetApiObject();
            } catch (ValidationException e) {
                // Walidacja danych nie powiodła się, generujemy odpowiedź błędu
                var result = new StandardCommunicateSerializer();
                result.CommunicateType = StandardCommunicateSerializer.TYPE_REQUEST_ERROR;
                result.Data = e.GetJson();

                return result.GetApiObject();
            }
        }
    }
}

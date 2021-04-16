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

        /// <summary>
        /// Odpowiedź dla zapytania autoryzacji zgodna ze standardem StandardCommunicateSerializer
        /// </summary>
        /// <returns>Obiekt JSON do przekazania do klienta</returns>
        protected override JObject GetAuthorizationResponseSuccessful() {
            var result = new StandardCommunicateSerializer();
            result.CommunicateType = StandardCommunicateSerializer.TYPE_AUTHORIZATION;

            var response = new StandardResponseSerializer();
            response.Status = "OK";
            response.Message = "Authorization successful";
            result.Data = response.GetApiObject();

            return result.GetApiObject();
        }
        /// <summary>
        /// Odpowiedź dla zapytania autoryzacji zgodna ze standardem StandardCommunicateSerializer
        /// </summary>
        /// <returns>Obiekt JSON do przekazania do klienta</returns>
        protected override JObject GetAuthorizationResponseFailed() {
            var result = new StandardCommunicateSerializer();
            result.CommunicateType = StandardCommunicateSerializer.TYPE_AUTHORIZATION;

            var response = new StandardResponseSerializer();
            response.Status = "FORBIDDEN";
            response.Message = "Authorization failed";
            result.Data = response.GetApiObject();

            return result.GetApiObject();
        }
        /// <summary>
        /// Treść komunikatu przy odłączeniu klienta od serwera przez zbyt długi czas autoryzacji
        /// </summary>
        /// <returns>Obiekt JSON do przekazania do klienta</returns>
        protected override JObject GetAuthorizationTimeoutSignal() {
            var resp = new StandardResponseSerializer() {
                Status = "AUTHORIZATION_TIMEOUT",
                Message = "Authorization failed - timeout"
            };
            var result = new StandardCommunicateSerializer() {
                CommunicateType = StandardCommunicateSerializer.TYPE_AUTHORIZATION,
                Data = resp.GetApiObject()
            };
            return result.GetApiObject();
        }
        /// <summary>
        /// Treść komunikatu przy odłączeniu klienta od serwera
        /// </summary>
        /// <returns>Obiekt JSON do przekazania do klienta</returns>
        protected override JObject GetDisconnectedSignal() {
            var resp = new StandardResponseSerializer() {
                Status = "DISCONNECTED",
                Message = "You have been disconnected"
            };
            var result = new StandardCommunicateSerializer() {
                CommunicateType = StandardCommunicateSerializer.TYPE_SERVER_SIGNAL,
                Data = resp.GetApiObject()
            };
            return result.GetApiObject();
        }
        /// <summary>
        /// Metoda autoryzująca połączenie klienckie. Po poprawnej autoryzacji, łączy z odpowiednim Lobby
        /// </summary>
        /// <param name="conn">Połączenie klienta</param>
        /// <param name="requestData">Dane zapytania (dane autoryzacyjne)</param>
        /// <returns>Informacja, czy autoryzacja zakończyła się powodzeniem</returns>
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
        /// <summary>
        /// Obsługa zapytań klienta
        /// </summary>
        /// <param name="conn">Połączenie klienta</param>
        /// <param name="requestData">Dane przychodzące od klienta</param>
        /// <returns></returns>
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
                result.RequestCode = initialCheck.RequestCode;
                result.Data = e.GetJson();

                return result.GetApiObject();
            }
        }
    }
}

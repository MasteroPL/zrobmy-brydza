using EasyHosting.Meta.Validators;
using EasyHosting.Models.Actions;
using EasyHosting.Models.Serialization;
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
        /// Metoda autoryzująca połączenie klienckie. Po poprawnej autoryzacji, łączy z odpowiednim Lobby
        /// </summary>
        /// <param name="conn">Połączenie klienta</param>
        /// <param name="requestData">Dane zapytania (dane autoryzacyjne)</param>
        /// <returns>Informacja, czy autoryzacja zakończyła się powodzeniem</returns>
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
        /// <summary>
        /// Obsługa zapytań klienta
        /// </summary>
        /// <param name="conn">Połączenie klienta</param>
        /// <param name="requestData">Dane przychodzące od klienta</param>
        /// <returns></returns>
        protected override JObject HandleRequest(ClientConnection conn, JObject requestData) {
            // Przekazanie danych do menadżera akcji
            try {
                var lobby = (Lobby)conn.Session.Get("joined-lobby");
                var response = lobby.ActionsManager.PerformActions(conn, requestData);

                return response;
            } catch (ValidationException e) {
                // Walidacja danych nie powiodła się, generujemy odpowiedź błędu
                return e.GetJson();
            }
        }
    }
}

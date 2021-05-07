using EasyHosting.Models.Server;
using ServerSocket.Models.Exceptions;
using ServerSocket.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Models {
    /// <summary>
    /// Klasa do zarządzania pokojami w lokalnej przestrzeni gniazda. Domyślnie tworzone jest lobby "DEFAULT", bez hasła
    /// </summary>
    public class LobbiesManager {
        public static event EventHandler<Lobby> LobbyCreated;
        public static event EventHandler<Lobby> LobbyClosed;

        public Dictionary<string, Lobby> Lobbies = new Dictionary<string, Lobby>();

        public LobbiesManager(bool createDefaultLobby = true, string defaultLobbyPassword = "") {
            if (createDefaultLobby) {
                Lobbies.Add("DEFAULT", new Lobby("DEFAULT", password: defaultLobbyPassword, parentManager:this));
            }
        }

        /// <summary>
        /// Tworzy nowe Lobby w przestrzeni gniazda
        /// </summary>
        /// <param name="lobbyId">Identyfikator Lobby</param>
        /// <param name="lobbyPassword">Hasło lobby</param>
        /// <exception cref="DuplicateLobbyIdException">Wyrzucany jeśli Lobby o takim identifikatorze już istnieje</exception>
        public void CreateLobby(string lobbyId, string lobbyPassword = "") {
            if (Lobbies.ContainsKey(lobbyId)) {
                throw new DuplicateLobbyIdException();
            }

            var newLobby = new Lobby(lobbyId, lobbyPassword, this);
            Lobbies.Add(lobbyId, newLobby);
            LobbyCreated?.Invoke(this, newLobby);
        }

        /// <summary>
        /// Zamyka wybrane Lobby, jeśli istnieje
        /// </summary>
        /// <param name="lobbyId">Identyfikator Lobby do zamknięcia</param>
        /// <exception cref="KeyNotFoundException">Wyjątek wyrzucany jeśli wybrane Lobby nie istnieje</exception>
        public void CloseLobby(string lobbyId) {
            if (!Lobbies.ContainsKey(lobbyId)) {
                throw new KeyNotFoundException("Lobby " + lobbyId + " doesn't exist");
            }

            var lobby = Lobbies[lobbyId];
            lobby.Close();

            Lobbies.Remove(lobbyId);
            LobbyClosed?.Invoke(this, lobby);
        }

        /// <summary>
        /// Dołącza wybranego klienta do Lobby (nie jest wykonywana autoryzacja)
        /// </summary>
        /// <param name="conn">Połączenie klienta</param>
        /// <param name="lobbyId">Identyfikator lobby</param>
        /// <exception cref="LobbyDoesNotExistException">Wyjątek wyrzucany jeżeli wybrane Lobby nie istnieje</exception>
        public void JoinLobby(ClientConnection conn, string lobbyId) {
            if (!Lobbies.ContainsKey(lobbyId)) {
                throw new LobbyDoesNotExistException();
            }

            var lobby = Lobbies[lobbyId];

            lobby.Join(conn);
            var data = new LobbySignalUserJoinedSerializer() {
                Signal = "USER_JOINED",
                Message = "Użytkownik " + conn.Session.Get("username") + " dołączył do Lobby",
                Username = (string)conn.Session.Get("username")
            };
            lobby.Broadcast(data.GetApiObject());
        }

        /// <summary>
        /// Dołącza wybranego klienta do Lobby jeśli hasło do Lobby jest poprawne
        /// </summary>
        /// <param name="conn">Połączenie klienta</param>
        /// <param name="lobbyId">Identyfikator Lobby</param>
        /// <param name="lobbyPassword">Hasło do Lobby</param>
        public void JoinLobby(ClientConnection conn, string lobbyId, string lobbyPassword) {
            if (!Lobbies.ContainsKey(lobbyId)) {
                throw new LobbyDoesNotExistException();
            }

            var lobby = Lobbies[lobbyId];

            if(lobby.Password != lobbyPassword) {
                throw new LobbyAuthorizationFailedException();
            }

            lobby.Join(conn);
        }
        
    }
}

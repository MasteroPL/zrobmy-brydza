using EasyHosting.Models.Server;
using ServerSocket.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Models {
    public class LobbiesManager {
        public Dictionary<string, Lobby> Lobbies = new Dictionary<string, Lobby>();

        public LobbiesManager() {
            Lobbies.Add("DEFAULT", new Lobby());
        }

        public void CreateLobby(string lobbyId, string lobbyPassword = "") {
            if (Lobbies.ContainsKey(lobbyId)) {
                throw new DuplicateLobbyIdException();
            }

            Lobbies.Add(lobbyId, new Lobby(lobbyPassword));
        }

        public void JoinLobby(ClientConnection conn, string lobbyId, string lobbyPassword = "") {
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

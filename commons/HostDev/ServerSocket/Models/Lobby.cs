using EasyHosting.Models.Actions;
using EasyHosting.Models.Server;
using EasyHosting.Models.Server.Config;
using ServerSocket.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameManagerLib.Models;
using Newtonsoft.Json.Linq;
using ServerSocket.Serializers;

namespace ServerSocket.Models {
    public class Lobby : IDisposable {
        private bool _Disposed = false;
        public bool Disposed {
            get { return _Disposed; }
        }

        public event EventHandler Closed;

        public List<ClientConnection> ConnectedClients = new List<ClientConnection>();
        public ActionsManager ActionsManager;
        public Match Game;
        public string Password;
        public ClientConnection LobbyOwner = null;

        public Lobby(string password = "") {
            // Lista akcji definiowana w konfiguracji
            ActionsManager = new ActionsManager(MainConfig.GAME_ACTIONS);
            Password = password;

            Game = new Match();
        }
        ~Lobby() {
            Dispose();
        }

        public void Join(ClientConnection newConnection) {
            if (newConnection.Session.Has("joined-lobby")) {
                var lobby = newConnection.Session.Get("joined-lobby");
                if(lobby != null) {
                    throw new LobbyException("Cannot join two lobbies at once");
                }
            }

            if (ConnectedClients.Contains(newConnection)) {
                throw new LobbyException("Already joined this lobby");
            }

            if(LobbyOwner == null) {
                LobbyOwner = newConnection;
            }

            string username = newConnection.Session.Get<string>("username");
            Console.WriteLine("Player " + username + " joined the lobby!");
            newConnection.Session.Set("joined-lobby", this);

            // Broadcast o dołączeniu nowego gracza do Lobby
            var signal = new LobbySignalUserJoinedSerializer() {
                Signal = LobbySignalUserJoinedSerializer.SIGNAL_USER_JOINED,
                Message = "User " + username + " joined the lobby",
                Username = username
            };
            var result = new StandardCommunicateSerializer() {
                CommunicateType = StandardCommunicateSerializer.TYPE_LOBBY_SIGNAL,
                Data = signal.GetApiObject()
            };
            Broadcast(result.GetApiObject());
            ConnectedClients.Add(newConnection);
        }
        /// <summary>
        /// Zamyka Lobby i wysyła informację do graczy o zamknięciu
        /// </summary>
        public void Close() {
            // Wysyłanie sygnału o zamknięciu Lobby
            var communicate = new StandardCommunicateSerializer();
            var serializer = new LobbySignalsSerializer();
            serializer.Signal = LobbySignalsSerializer.SIGNAL_CLOSED;
            serializer.Message = "Lobby has been closed";
            communicate.CommunicateType = StandardCommunicateSerializer.TYPE_LOBBY_SIGNAL;
            communicate.Data = serializer.GetApiObject();
            Broadcast(communicate.GetApiObject());

            foreach(var client in ConnectedClients) {
                try {
                    MainConfig.SERVER_SOCKET.DisconnectClient(client);
                } catch (ArgumentException) {
                    Console.WriteLine("Client " + client.ToString() + " not connected, cannot disconnect.");
                }
            }

            Closed?.Invoke(this, null);

            Dispose();
        }

        public void Broadcast(JObject communicate) {
            foreach(var client in ConnectedClients) {
                client.AddCommunicate(communicate);
            }
        }

        public virtual void Dispose() {
            if (!Disposed) {
                this.LobbyOwner = null;
                this.ConnectedClients.Clear();
                this.ConnectedClients = null;

                _Disposed = true;
            }
        }
    }
}

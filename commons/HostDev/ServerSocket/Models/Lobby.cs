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
using EasyHosting.Models.Serialization;

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
        public string Id { private set; get; }
        public LobbiesManager ParentManager { private set; get; }

        public LobbyState LobbyState { private set; get; }

        public bool UsernameAlreadyJoined(string username) {
            foreach(var client in ConnectedClients) {
                if(client.Session.Get<string>("username").CompareTo(username) == 0) {
                    return true;
                }
            }
            return false;
        }

        public Lobby(string lobbyId, string password = "", LobbiesManager parentManager = null) {
            this.Id = lobbyId;
            this.ParentManager = parentManager;

            // Lista akcji definiowana w konfiguracji
            ActionsManager = new ActionsManager(MainConfig.GAME_ACTIONS);
            Password = password;
            LobbyState = LobbyState.IDLE;

            Game = new Match();
        }
        ~Lobby() {
            Dispose();
        }
        /// <summary>
        /// Zmienia stan lobby jeśli jest inny niż podany w argumencie "newState"
        /// </summary>
        /// <param name="newState">Nowy stan do ustawienia</param>
        /// <param name="sendBroadcast">Określa czy należy wysłać broadcast do graczy o zmianie stanu lobby (jeżeli nastąpi zmiana stanu)</param>
        public void SetLobbyState(LobbyState newState, bool sendBroadcast=true) {
            if(newState != LobbyState) {
                LobbyState = newState;

                if (sendBroadcast) {
                    // Broadcast do graczy
                    var broadcastData = new LobbySignalLobbyStateChangedSerializer() {
                        Signal = LobbySignalLobbyStateChangedSerializer.SIGNAL_LOBBY_STATE_CHANGED,
                        LobbyState = (int)LobbyState
                    };
                    var broadcastWrapper = new StandardCommunicateSerializer() {
                        CommunicateType = StandardCommunicateSerializer.TYPE_LOBBY_SIGNAL,
                        Data = broadcastData.GetApiObject()
                    };
                    Broadcast(broadcastWrapper.GetApiObject());
                }
            }
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

            if (UsernameAlreadyJoined(username)) {
                throw new UsernameTakenException("Username taken");
            }

            Console.WriteLine("Player " + username + " joined the lobby!");
            newConnection.Session.Set("joined-lobby", this);
            newConnection.BeforeDispose += OnClientConnectionDisposed;

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
        public void RemoveFromLobby(ClientConnection client) {
            if (!ConnectedClients.Contains(client)) {
                throw new ArgumentException("Client not connected to Lobby");
            }

            // Komunikat o opuszczeniu Lobby przez użytkownika
            string username = client.Session.Get<string>("username");
            
            var signal = new LobbySignalUserRemovedSerializer() {
                Signal = LobbySignalUserRemovedSerializer.SINGAL_USER_REMOVED,
                Message = "User " + username + " left the lobby",
                Username = username
            };
            var player = Game.GetPlayerByUsername(username);
            if (player != null) {
                Game.RemovePlayer(player);
                signal.WasSitted = true;
                signal.PlaceTag = (int)player.Tag;

                SetLobbyState(LobbyState.IDLE); // Użytkownik siedział przy stole, jeśli gra była w trakcie, należy ją zapauzować
            }
            else {
                signal.WasSitted = false;
            }

            var result = new StandardCommunicateSerializer() {
                CommunicateType = StandardCommunicateSerializer.TYPE_LOBBY_SIGNAL,
                Data = signal.GetApiObject()
            };
            Broadcast(result.GetApiObject());

            // Usuwamy klienta z Lobby
            ConnectedClients.Remove(client);
            client.BeforeDispose -= OnClientConnectionDisposed;
            Console.WriteLine("Użytkownik usunięty z Lobby: " + username);

            // Ustawianie nowego właściciela Lobby
            if (LobbyOwner == client) {
                if(ConnectedClients.Count > 0) {
                    LobbyOwner = ConnectedClients.First();
                }
                else {
                    if (ParentManager == null)
                        Close();
                    else
                        ParentManager.CloseLobby(Id);
                }
            }
            client.Dispose();
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

            Console.WriteLine("Lobby zamknięte: " + this.ToString());

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

        protected virtual void OnClientConnectionDisposed(object sender, EventArgs args) {
            ClientConnection conn = (ClientConnection)sender;

            try {
                RemoveFromLobby(conn);
            } catch (ArgumentException) { }
        }
    }

    /// <summary>
    /// Lobby może być w dwóch stanach:
    /// * IDLE - tj. rozgrywka nie jest rozpoczęta, lub z dowolnych przyczyn została zapauzowana
    /// * IN_GAME - tj. rozgrywka jest w trakcie trwania
    /// </summary>
    public enum LobbyState {
        IDLE = 0,
        IN_GAME = 1
    };
}

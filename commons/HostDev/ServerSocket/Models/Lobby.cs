﻿using EasyHosting.Models.Actions;
using EasyHosting.Models.Server;
using EasyHosting.Models.Server.Config;
using ServerSocket.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Models {
    public class Lobby {
        public List<ClientConnection> ConnectedClients = new List<ClientConnection>();
        public ActionsManager ActionsManager;
        public string Password;

        public Lobby(string password = "") {
            // Lista akcji definiowana w konfiguracji
            ActionsManager = new ActionsManager(MainConfig.GAME_ACTIONS);
            Password = password;
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

            ConnectedClients.Add(newConnection);
            Console.WriteLine("Player " + newConnection.Session.Get("username") + " joined the lobby!");
            newConnection.Session.Set("joined-lobby", this);
        }
    }
}
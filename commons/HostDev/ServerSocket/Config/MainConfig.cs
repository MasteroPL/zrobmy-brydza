using EasyHosting.Models.Actions;
using ServerSocket.Actions.Bid;
using ServerSocket.Actions.GetTableInfo;
using ServerSocket.Actions.HelloWorld;
using ServerSocket.Actions.Sit;
using ServerSocket.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Server.Config {
    public static class MainConfig {
        public static readonly Dictionary<string, BaseAction> GAME_ACTIONS = new Dictionary<string, BaseAction>() {
            { "hello-world", new HelloWorldAction() },
            { "get-table-info", new GetTableInfoAction() },
            { "sit", new SitAction() }
        };

        private static BridgeServerSocket ServerSocket = null;

        public static BridgeServerSocket SERVER_SOCKET {
            get {
                return ServerSocket;
            }
        }

        public static void InitiateServerSocket() {
            ServerSocket = new BridgeServerSocket();
        }
    }
}

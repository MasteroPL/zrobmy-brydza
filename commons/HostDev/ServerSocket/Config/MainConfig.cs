using EasyHosting.Models.Actions;
using ServerSocket.Actions.Bid;
using ServerSocket.Actions.GetTableInfo;
using ServerSocket.Actions.HelloWorld;
using ServerSocket.Actions.LeavePlace;
using ServerSocket.Actions.Sit;
using ServerSocket.Actions.StartGame;
using ServerSocket.Actions.SitPlayerOut;
using ServerSocket.Models;
using System;
using System.Collections.Generic;
using System.Text;
using ServerSocket.Actions.GetHand;
using ServerSocket.Actions.PutCard;
using ServerSocket.Actions.RetrieveTableData;
using ServerSocket.Actions.SendMessage;

namespace EasyHosting.Models.Server.Config {
    public static class MainConfig {
        public static readonly Dictionary<string, BaseAction> GAME_ACTIONS = new Dictionary<string, BaseAction>() {
            { "hello-world", new HelloWorldAction() },
            { "get-table-info", new GetTableInfoAction() },
            { "sit", new SitAction() },
            { "leave-place", new LeavePlaceAction() },
            { "start-game", new StartGameAction() },
            { "sit-player-out", new SitPlayerOutAction() },
            { "get-hand", new GetHandAction() },
            { "bid", new BidAction() },
            { "put-card", new PutCardAction() },
            { "retrieve-table-data", new RetrieveTableDataAction() },
            { "send-message", new SendMessageAction() }
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

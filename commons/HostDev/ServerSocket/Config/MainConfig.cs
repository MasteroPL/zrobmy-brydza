using EasyHosting.Models.Actions;
using ServerSocket.Actions.HelloWorld;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Server.Config {
    public static class MainConfig {
        public static readonly Dictionary<string, BaseAction> GAME_ACTIONS = new Dictionary<string, BaseAction>() {
            { "hello-world", new HelloWorldAction() }
        };
    }
}

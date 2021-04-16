using EasyHosting.Models.Server.Config;
using ServerSocket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket {
    class Program {
        static void Main(string[] args) {
            MainConfig.InitiateServerSocket();
            MainConfig.SERVER_SOCKET.Start();
        }
    }
}

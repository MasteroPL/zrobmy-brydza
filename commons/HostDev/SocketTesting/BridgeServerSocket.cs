using EasyHosting.Models.Server;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketTesting {
    public class BridgeServerSocket : ServerSocket {
        protected override bool AuthorizeConnection(ClientConnection conn, JObject requestData) {
            throw new NotImplementedException();
        }

        protected override JObject HandleRequest(ClientConnection conn, JObject requestData) {
            throw new NotImplementedException();
        }
    }
}

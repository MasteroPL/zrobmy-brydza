using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Models.Exceptions {
    public class ServerSynchronizationException : Exception {
        public ServerSynchronizationException() : base() { }
        public ServerSynchronizationException(string message) : base(message) { }
    }
}

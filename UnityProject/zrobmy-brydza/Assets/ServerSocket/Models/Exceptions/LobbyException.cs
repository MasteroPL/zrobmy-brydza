using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Models.Exceptions {
    public class LobbyException : Exception {
        public LobbyException() : base() { }
        public LobbyException(string message) : base(message) { }
        public LobbyException(string message, Exception innerException) : base(message, innerException) { }
        public LobbyException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

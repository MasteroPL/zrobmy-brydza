using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Models.Exceptions {
    class LobbyAuthorizationFailedException : LobbyException {
        public LobbyAuthorizationFailedException() : base() { }
        public LobbyAuthorizationFailedException(string message) : base(message) { }
        public LobbyAuthorizationFailedException(string message, Exception innerException) : base(message, innerException) { }
        public LobbyAuthorizationFailedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

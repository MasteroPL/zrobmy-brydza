using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Models.Exceptions {
    public class LobbyDoesNotExistException : LobbyException {
        public LobbyDoesNotExistException() : base() { }
        public LobbyDoesNotExistException(string message) : base(message) { }
        public LobbyDoesNotExistException(string message, Exception innerException) : base(message, innerException) { }
        public LobbyDoesNotExistException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

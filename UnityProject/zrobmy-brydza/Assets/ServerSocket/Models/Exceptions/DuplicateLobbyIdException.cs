using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Models.Exceptions {
    class DuplicateLobbyIdException : LobbyException {
        public DuplicateLobbyIdException() : base() { }
        public DuplicateLobbyIdException(string message) : base(message) { }
        public DuplicateLobbyIdException(string message, Exception innerException) : base(message, innerException) { }
        public DuplicateLobbyIdException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Models.Exceptions {
    public class UsernameTakenException : LobbyException {
        public UsernameTakenException() : base() { }
        public UsernameTakenException(string message) : base(message) { }
        public UsernameTakenException(string message, Exception innerException) : base(message, innerException) { }
        public UsernameTakenException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

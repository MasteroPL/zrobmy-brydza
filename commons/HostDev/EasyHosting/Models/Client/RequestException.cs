using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Client {
    public class RequestException : Exception {
        public RequestException() : base() { }
        public RequestException(string message) : base(message) { }
        public RequestException(string message, Exception innerException) : base(message, innerException) { }
        public RequestException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

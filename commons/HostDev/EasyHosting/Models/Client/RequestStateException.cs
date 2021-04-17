using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Client {
    public class RequestStateException : RequestException {
        public RequestStateException() : base() { }
        public RequestStateException(string message) : base(message) { }
        public RequestStateException(string message, Exception innerException) : base(message, innerException) { }
        public RequestStateException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

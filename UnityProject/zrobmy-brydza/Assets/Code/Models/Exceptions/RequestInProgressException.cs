using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Models.Exceptions {
    public class RequestInProgressException : ServerSynchronizationException {
        public RequestInProgressException() : base() { }
        public RequestInProgressException(string message) : base(message) { }
    }
}

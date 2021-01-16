using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManagerLib.Exceptions
{
    public class WrongBidException : Exception
    {
        public WrongBidException() : base() { }
        public WrongBidException(string message) : base(message) { }
    }
}

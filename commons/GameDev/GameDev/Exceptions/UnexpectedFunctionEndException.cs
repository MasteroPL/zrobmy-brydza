using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManagerLib.Exceptions
{
    class UnexpectedFunctionEndException : Exception
    {
        public UnexpectedFunctionEndException() : base() { }
        public UnexpectedFunctionEndException(string message) : base(message) { }
    }
}

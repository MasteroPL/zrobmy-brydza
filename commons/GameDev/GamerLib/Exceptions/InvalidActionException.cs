using System;
using System.Collections.Generic;
using System.Text;

namespace GameManagerLib.Exceptions
{
    public class InvalidActionException : Exception
    {
        public InvalidActionException() : base() { }
        public InvalidActionException(string message) : base(message) { }
    }
}

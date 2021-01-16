using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManagerLib.Exceptions
{
    class WrongGameStateException : Exception
    {
        public WrongGameStateException() : base() { }
        public WrongGameStateException(string message) : base(message) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManagerLib.Exceptions
{
    class WrongPlayerException : Exception
    {
        public WrongPlayerException() : base() { }
        public WrongPlayerException(string message) : base(message) {}
    }
}

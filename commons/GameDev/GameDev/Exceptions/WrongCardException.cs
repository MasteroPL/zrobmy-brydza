using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManagerLib.Exceptions
{
    class WrongCardException : Exception
    {
        public WrongCardException() : base() { }
        public WrongCardException(string message) : base(message) { }
    }
}

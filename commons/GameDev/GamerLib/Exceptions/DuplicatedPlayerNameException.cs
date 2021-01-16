using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManagerLib.Exceptions
{
    class DuplicatedPlayerNameException : Exception
    {
        public DuplicatedPlayerNameException() : base() { }
        public DuplicatedPlayerNameException(string message) : base(message) { }
    }
}

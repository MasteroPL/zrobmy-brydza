using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManagerLib.Exceptions
{
    public class SeatTakenException : Exception
    {
        public SeatTakenException() : base() { }
        public SeatTakenException(string message) : base(message) { }
    }
}

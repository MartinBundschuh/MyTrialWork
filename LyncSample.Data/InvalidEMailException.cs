using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncSample.Data
{
    /// <summary>
    /// Exception used if the given E-Mail Contact is not valid.
    /// </summary>
    public class InvalidEMailException : Exception
    {
        public InvalidEMailException() { }

        public InvalidEMailException(string message)
            : base(message) { }

        public InvalidEMailException(string message, Exception inner)
            : base(message, inner) { }
    }
}

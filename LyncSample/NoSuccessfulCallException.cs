using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncSample
{
    /// <summary>
    /// Exception used if the Lync-Call couldnt be proceeded.
    /// </summary>
    public class NoSuccessfulCallException : Exception
    {
        public NoSuccessfulCallException() { }

        public NoSuccessfulCallException(string message)
            : base(message) { }

        public NoSuccessfulCallException(string message, Exception inner)
            : base(message, inner) { }
    }
}

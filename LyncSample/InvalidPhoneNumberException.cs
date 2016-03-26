using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncSample
{
    /// <summary>
    /// Exception used if the given Phonenumber is not valid.
    /// </summary>
    public class InvalidPhoneNumberException : Exception
    {
        public InvalidPhoneNumberException() { }

        public InvalidPhoneNumberException(string message)
            : base(message) { }

        public InvalidPhoneNumberException(string message, Exception inner)
            : base(message, inner) { }
    }
}

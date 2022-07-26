using System;

namespace LyncSample.Data
{
    /// <summary>
    /// Exception used if the given PhoneNumber is not valid.
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

using System;

namespace Checkables
{
    public class CheckableException : InvalidOperationException
    {
        public CheckableException() { }

        public CheckableException(string message)
            : base(message) { }

        public CheckableException(string message, Exception inner)
            : base(message, inner) { }
    }
}

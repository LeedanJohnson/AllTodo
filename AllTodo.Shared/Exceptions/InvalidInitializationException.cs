using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.Shared.Exceptions
{
    public class InvalidInitializationException : Exception
    {
        public InvalidInitializationException(string message)
            : base(message) { }
    }
}

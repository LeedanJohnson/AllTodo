using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using AllTodo.Shared.Exceptions;

namespace SecureCodingToDo
{
    public class Username
    {
        private static readonly int MINIMUM_LENGTH = 1;
        private static readonly int MAXIMUM_LENGTH = 80;
        private static readonly Regex VALIDITY_REGEX = new Regex("^[A-Za-z\']+$");

        public Username(string value)
        {
            value = value.Trim();

            if (value.Length < MINIMUM_LENGTH || value.Length > MAXIMUM_LENGTH)
                throw new InvalidInitializationException("Invalid Length of UserName");

            if (!VALIDITY_REGEX.IsMatch(value))
                throw new InvalidInitializationException("Username contained invalid characters");

            this.value = value;
        }
        
        private readonly string value;
        public string Value
        {
            get { return Value; }
        }
    }
}

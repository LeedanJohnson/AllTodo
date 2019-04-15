using AllTodo.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SecureCodingToDo
{
    class PhoneNumber
    {

        private static readonly int MINIMUM_LENGTH = 11;
        private static readonly int MAXIMUM_LENGTH = 12;
        private static readonly Regex VALIDITY_REGEX = new Regex("^[0-9]");

        public PhoneNumber(string number)
        {
            number = number.Trim();

            if (number.Length < MINIMUM_LENGTH || number.Length > MAXIMUM_LENGTH)
                throw new InvalidInitializationException("Invalid Length of Phone Number");
        }

        private readonly string number;
        public string Number
        {
            get { return number; }
        }

    }
}

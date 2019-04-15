using AllTodo.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AllTodo.Shared.Models
{
    class PhoneNumber
    {
        private static readonly int REQUIRED_LENGTH = 12;
        private static readonly Regex VALIDITY_REGEX = new Regex("^[0-9]");

        public PhoneNumber(string value)
        {
            this.value = value.Trim();

            if (this.value.Length != REQUIRED_LENGTH)
                throw new InvalidInitializationException("Invalid Length of Phone Number");
        }

        private readonly string value;
        public string Value
        {
            get { return this.value; }
        }

    }
}

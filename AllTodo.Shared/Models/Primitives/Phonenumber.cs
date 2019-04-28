using AllTodo.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AllTodo.Shared.Models
{
    public class PhoneNumberDTO
    {
        private string value;
        public string Value { get { return this.value; } set { this.value = value.Trim(); } }

        public PhoneNumberDTO(string value)
        {
            this.value = value.Trim();
        }

        public (bool success, string message) Validate()
        {
            return PhoneNumber.Validate(this.value);
        }

        override
        public string ToString()
        {
            return this.value;
        }

        public PhoneNumber GetObject()
        {
            return new PhoneNumber(this.value);
        }
    }

    public class PhoneNumber
    {

        public PhoneNumber(string value)
        {
            value = value.Trim();
            var validation_result = PhoneNumber.Validate(value);
            if (!validation_result.success)
                throw new InvalidInitializationException(validation_result.message);
            this.value = value;
        }

        private readonly string value;
        public string Value
        {
            get { return this.value; }
        }

        // TODO: Finish validity check
        private static readonly int REQUIRED_LENGTH = 11;
        private static readonly Regex VALIDITY_REGEX = new Regex("^[0-9]");
        public static (bool success, string message) Validate(string value)
        {
            if (value.Length != REQUIRED_LENGTH)
                return (false, $"Invalid Length of Phone Number. Expected {REQUIRED_LENGTH}, got {value.Length}.");
            return (true, "Validation Successful");
        }

        public PhoneNumberDTO GetDTO()
        {
            return new PhoneNumberDTO(this.value);
        }

    }
}

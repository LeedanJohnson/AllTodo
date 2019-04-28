using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using AllTodo.Shared.Exceptions;

namespace AllTodo.Shared.Models.Primitives
{
    public class UsernameDTO
    {
        private string value;
        public string Value { get { return this.value; } set { this.value = value.Trim().ToLower(); } }

        public UsernameDTO(string value)
        {
            this.value = value.Trim().ToLower();
        }

        public (bool success, string message) Validate()
        {
            return Username.Validate(this.value);
        }

        override
        public string ToString()
        {
            return this.value;
        }

        public Username GetObject()
        {
            return new Username(this.value);
        }
    }

    public class Username : DomainPrimitive<Username>
    {

        public Username(string value)
        {
            value = value.Trim().ToLower();
            var validation_result = Username.Validate(value);
            if (!validation_result.success)
                throw new InvalidInitializationException(validation_result.message);
            this.value = value;
        }

        private static readonly int MIN_LENGTH = 4;
        private static readonly int MAX_LENGTH = 40;
        private static readonly Regex VALIDITY_REGEX = new Regex("^[a-z\']+$");
        public static (bool success, string message) Validate(string value)
        {
            if (value.Length < MIN_LENGTH || value.Length > MAX_LENGTH)
                return (false, $"Invalid Length of UserName. Expected {MIN_LENGTH}-{MAX_LENGTH}, got {value.Length}.");

            if (!VALIDITY_REGEX.IsMatch(value))
                return (false, "Username contained invalid characters");

            return (true, "Validation Successful");
        }
        
        private readonly string value;
        public string Value
        {
            get { return this.value; }
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType().Equals(typeof(Username)))
            {
                Username other = (Username)obj;
                if (other.Value.Equals(this.Value))
                    return true;
                return false;
            }
            return false;
        }
    }
}

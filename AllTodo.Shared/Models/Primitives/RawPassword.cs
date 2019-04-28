using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AllTodo.Shared.Models.Primitives
{
    public class RawPassword
    {
        // TODO: Fill in regex
        private static readonly int MIN_LENGTH = 6;
        private static readonly int MAX_LENGTH = 30;
        private static readonly Regex VALIDITY_REGEX = new Regex("^*$");
        public static (bool success, string message) Validate(string password)
        {
            if (password == null)
                return (false, "Password cannot be null");
            if (password.Length < MIN_LENGTH || password.Length > MAX_LENGTH)
                return (false, $"Invalid Length of Password. Expected {MIN_LENGTH}-{MAX_LENGTH}, got {password.Length}.");
            if (!VALIDITY_REGEX.IsMatch(password))
                return (false, $"Password contained invalid characters: {password}");
            return (true, "Validation Successful");
        }
    }
}

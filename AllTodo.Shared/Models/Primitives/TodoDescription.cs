using AllTodo.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AllTodo.Shared.Models.Primitives
{
    public class TodoDescriptionDTO
    {
        private string value;
        public string Value { get { return this.value; } set { this.value = value.Trim(); } }

        public TodoDescriptionDTO(string value)
        {
            this.value = value.Trim();
        }

        public (bool success, string message) Validate()
        {
            return TodoDescription.Validate(this.value);
        }

        override
        public string ToString()
        {
            return this.value;
        }

        public TodoDescription GetObject()
        {
            return new TodoDescription(this.value);
        }
    }


    public class TodoDescription
    {
        private readonly string value;
        public string Value { get { return this.value; } }

        public TodoDescription(string value)
        {
            value = value.Trim();
            var validation_result = TodoDescription.Validate(value);
            if (!validation_result.success)
                throw new InvalidInitializationException(validation_result.message);
            this.value = value;
        }

        // TODO: Fill in regex
        private static readonly int MIN_LENGTH = 6;
        private static readonly int MAX_LENGTH = 400;
        private static readonly Regex VALIDITY_REGEX = new Regex("^*$");
        public static (bool success, string message) Validate(string value)
        {
            if (value == null)
                return (false, "Description cannot be null");
            if (value.Length < MIN_LENGTH || value.Length > MAX_LENGTH)
                return (false, $"Invalid Length of Description. Expected {MIN_LENGTH}-{MAX_LENGTH}, got {value.Length}.");
            if (!VALIDITY_REGEX.IsMatch(value))
                return (false, $"Description contained invalid characters: {value}");
            return (true, "Validation Successful");
        }

        override
        public string ToString()
        {
            return this.value;
        }

        public TodoDescriptionDTO GetDTO()
        {
            return new TodoDescriptionDTO(this.value);
        }
    }
}

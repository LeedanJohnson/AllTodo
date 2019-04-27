using AllTodo.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AllTodo.Shared.Models.Primitives
{
    public class TodoTitleDTO
    {
        private string value;
        public string Value { get { return this.value; } set { this.value = value.Trim(); } }

        public TodoTitleDTO(string value)
        {
            this.value = value.Trim();
        }

        public (bool success, string message) Validate()
        {
            return TodoTitle.Validate(this.value);
        }

        override
        public string ToString()
        {
            return this.value;
        }

        public TodoTitle GetObject()
        {
            return new TodoTitle(this.value);
        }
    }


    public class TodoTitle
    {
        private readonly string value;
        public string Value { get { return this.value; } }

        public TodoTitle(string value)
        {
            value = value.Trim().ToLower();
            var validation_result = TodoTitle.Validate(value);
            if (!validation_result.success)
                throw new InvalidInitializationException(validation_result.message);
            this.value = value;
        }

        // TODO: Fill in regex
        private static readonly int MIN_LENGTH = 3;
        private static readonly int MAX_LENGTH = 20;
        private static readonly Regex VALIDITY_REGEX = new Regex("^*$");
        public static (bool success, string message) Validate(string value)
        {
            if (value == null)
                return (false, "Title cannot be null");
            if (value.Length < MIN_LENGTH || value.Length > MAX_LENGTH)
                return (false, $"Invalid Length of Title: {value.Length}");
            if (!VALIDITY_REGEX.IsMatch(value))
                return (false, $"Title contained invalid characters: {value}");
            return (true, "Validation Successful");
        }

        override
        public string ToString()
        {
            return this.value;
        }

        public TodoTitleDTO GetDTO()
        {
            return new TodoTitleDTO(this.value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using AllTodo.Shared.Exceptions;

namespace SecureCodingToDo
{
    public class UserName
    {

        private static readonly int MINIMUM_LENGTH = 1;
        private static readonly int MAXIMUM_LENGTH = 80;
        private static readonly Regex VALIDITY_REGEX = new Regex("^[A-Za-z\']+$");

        public UserName(string username)
        {
            username = username.Trim();

            if (username.Length < MINIMUM_LENGTH || username.Length > MAXIMUM_LENGTH)
                throw new InvalidInitializationException("Invalid Length of UserName");

            if (!VALIDITY_REGEX.IsMatch(username))
                throw new InvalidInitializationException("Username contained invalid characters");

            this.username = username;
        }

        // Where did this come from?
        // [Required]
        // [MaxLength(100)]
        private readonly string username;
        public string Username
        {
            get
            {
                return username;
            }
        }
    }
}

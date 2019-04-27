using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AllTodo.Shared.Models
{
    public class HashedPassword
    {
        // TODO: fill in the regex
        private static readonly int MINIMUM_LENGTH = 8;
        private static readonly int MAXIMUM_LENGTH = 25;
        private static readonly Regex VALIDITY_REGEX = new Regex("^*$");

        public HashedPassword(string password)
        {
            this.hash = BCrypt.Net.BCrypt.HashPassword(password);
        }

        private readonly string hash;
        public string Hash
        {
            get { return this.hash; }
        }

        public bool Verify(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, this.hash);
        }
    }
}

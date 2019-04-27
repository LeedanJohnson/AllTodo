using AllTodo.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllTodo.Shared.Models.Primitives
{
    public class AuthToken
    {
        private readonly string token;
        public string Token { get { return this.token; } }

        public AuthToken()
        {
            string guid = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            token = BCrypt.Net.BCrypt.EnhancedHashPassword(guid, BCrypt.Net.HashType.SHA256);
        }

        // TODO: Validate token
        public AuthToken(string token)
        {
            this.token = token;
        }

        // TODO: Overload equality

        public bool Matches(AuthToken other)
        {
            return this.token == other.Token;
        }
    }
}

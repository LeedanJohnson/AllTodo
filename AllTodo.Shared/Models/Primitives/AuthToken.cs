using AllTodo.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllTodo.Shared.Models.Primitives
{
    public class AuthToken
    {
        string token;

        public AuthToken()
        {
            string guid = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            token = BCrypt.Net.BCrypt.EnhancedHashPassword(guid, BCrypt.Net.HashType.SHA256);
        }

        public bool Matches(string candidate)
        {
            return candidate == token;
        }
    }
}

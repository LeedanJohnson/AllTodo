using AllTodo.Shared.Exceptions;
using AllTodo.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllTodo.Shared.Models.Primitives
{
    public class AuthTokenDTO
    {
        public string Token { get; set; }

        public AuthTokenDTO(string token)
        {
            this.Token = token;
        }

        public (bool success, string message) Validate()
        {
            return AuthToken.Validate(this.Token);
        }

        override
        public string ToString()
        {
            return this.Token;
        }

        public AuthToken GetObject()
        {
            return new AuthToken(this.Token);
        }
    }


    public class AuthToken
    {
        private readonly string token;
        public string Token { get { return this.token; } }

        public static AuthToken Generate()
        {
            string guid = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            string token = BCrypt.Net.BCrypt.EnhancedHashPassword(guid, BCrypt.Net.HashType.SHA256);
            return new AuthToken(token);
        }

        public AuthToken(string token)
        {
            var validation_result = AuthToken.Validate(token);
            if (!validation_result.success)
                throw new InvalidInitializationException(validation_result.message);
            this.token = token;
        }

        // TODO: Overload equality

        public bool Matches(AuthToken other)
        {
            return this.token == other.Token;
        }

        // TODO: Validate token
        public static (bool success, string message) Validate(string token)
        {
            return (true, "Validation Successful");
        }

        public AuthTokenDTO GetDTO()
        {
            return new AuthTokenDTO(this.token);
        }
    }
}

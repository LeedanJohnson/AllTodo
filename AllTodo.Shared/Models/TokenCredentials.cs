using AllTodo.Shared.Models.Primitives;
using AllTodo.Shared.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.Shared.Models
{
    public class TokenCredentialsDTO
    {
        public string IDToken;
        public string UserAuthToken;

        public TokenCredentialsDTO() { }

        public TokenCredentialsDTO(string idtoken, string authtoken)
        {
            this.IDToken = idtoken;
            this.UserAuthToken = authtoken;
        }

        public (bool success, string message) Validate()
        {
            var idtoken_validation = MachineIDToken.Validate(IDToken);
            if (!idtoken_validation.success)
                return idtoken_validation;

            var authtoken_validation = AuthToken.Validate(UserAuthToken);
            if (!authtoken_validation.success)
                return authtoken_validation;

            return TokenCredentials.Validate(new MachineIDToken(IDToken, null), new AuthToken(UserAuthToken));
        }

        override
        public string ToString()
        {
            return $"IDToken: {IDToken.ToString()}, AuthToken: {UserAuthToken.ToString()}";
        }

        public TokenCredentials GetObject(IDateTimeProvider datetime_provider)
        {
            return new TokenCredentials(new MachineIDToken(IDToken, datetime_provider), new AuthToken(UserAuthToken));
        }
    }

    public class TokenCredentials
    {
        public MachineIDToken IDToken { get; set; }
        
        public AuthToken UserAuthToken { get; set; }

        public TokenCredentials(MachineIDToken idtoken, AuthToken authtoken)
        {
            this.IDToken = idtoken;
            this.UserAuthToken = authtoken;
        }

        public static (bool success, string message) Validate(MachineIDToken id_token, AuthToken auth_token)
        {
            if (id_token is null)
                return (false, "IDToken cannot be null");

            if (auth_token is null)
                return (false, "AuthToken cannot be null");

            return (true, "Validation Successful");
        }

        override
        public string ToString()
        {
            return $"IDToken: {IDToken.ToString()}, AuthToken: {UserAuthToken.ToString()}";
        }

        public TokenCredentialsDTO GetDTO()
        {
            TokenCredentialsDTO dto = new TokenCredentialsDTO();
            dto.IDToken = this.IDToken.Token;
            dto.UserAuthToken = this.UserAuthToken.Token;
            return dto;
        }
    }
}

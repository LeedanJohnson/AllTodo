using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.Shared.Models.Primitives
{
    public class TokenCredentialsDTO
    {
        public string IDToken;
        public string AuthToken;

        public TokenCredentialsDTO(string idtoken, string authtoken)
        {
            this.IDToken = idtoken;
            this.AuthToken = authtoken;
        }

        public string ToString()
        {
            return $"IDToken: {this.IDToken}, AuthToken: {this.AuthToken}";
        }
    }

    public class TokenCredentials
    {
        private readonly MachineIDToken idtoken;
        public MachineIDToken IDToken { get { return idtoken; } }

        private readonly AuthToken authtoken;
        public AuthToken AuthToken { get { return authtoken; } }

        public TokenCredentials(MachineIDToken idtoken, AuthToken authtoken)
        {
            this.idtoken = idtoken;
            this.authtoken = authtoken;
        }

        public TokenCredentialsDTO GetDTO()
        {
            return new TokenCredentialsDTO(this.idtoken.Token, this.authtoken.Token);
        }

        public string ToString()
        {
            return $"IDToken: {this.idtoken.Token}, AuthToken: {this.authtoken.Token}";
        }
    }
}

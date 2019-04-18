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
    }
}

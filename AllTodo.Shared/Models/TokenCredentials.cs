using AllTodo.Shared.Models.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.Shared.Models
{
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

        override
        public string ToString()
        {
            return $"IDToken: {idtoken.ToString()}, AuthToken: {authtoken.ToString()}";
        }
    }
}

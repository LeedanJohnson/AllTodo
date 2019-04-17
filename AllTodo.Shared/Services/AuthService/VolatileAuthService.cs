using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllTodo.Shared.Models;
using AllTodo.Shared.Models.Primitives;

namespace AllTodo.Shared.Services
{
    public class VolatileAuthService : IAuthService
    {
        private IDateTimeProvider datetime_provider;
        private IUserService userservice;
        private IDictionary<MachineIDToken, (AuthToken authtoken, Username username)> tokenized_users;

        public VolatileAuthService(IDateTimeProvider datetime_provider, IUserService userservice)
        {
            this.userservice = userservice;
            this.datetime_provider = datetime_provider;
        }

        public bool ValidateCredentials(Username username, string password, out User user)
        {
            user = null;
            if (userservice.Exists(username))
            {
                if (userservice.GetUserPassword(username).Verify(password))
                {
                    user = userservice.GetUser(username);
                    return true;
                }
            }
            return false;
        }

        public (MachineIDToken idtoken, AuthToken authtoken) GenerateTokens(User user)
        {
            MachineIDToken idtoken = new MachineIDToken(this.datetime_provider);
            AuthToken authtoken = new AuthToken();

            tokenized_users.Add(idtoken, (authtoken, user.Username));

            return (idtoken, authtoken);
        }

        public bool ValidateTokens(string idtoken, string authtoken)
        {

        }
    }
}

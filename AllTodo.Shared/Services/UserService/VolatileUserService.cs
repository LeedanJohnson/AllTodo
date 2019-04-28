using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllTodo.Shared.Models;
using AllTodo.Shared.Models.Primitives;

namespace AllTodo.Shared.Services
{
    public class UsernameComparer : IEqualityComparer<Username>
    {
        public bool Equals(Username x, Username y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(Username obj)
        {
            return obj.Value.GetHashCode();
        }
    }

    public class MachineIDTokenComparer : IEqualityComparer<MachineIDToken>
    {
        public bool Equals(MachineIDToken x, MachineIDToken y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(MachineIDToken obj)
        {
            return obj.Token.GetHashCode();
        }
    }

    public class VolatileUserService : IUserService
    {
        private int current_id;
        IDictionary<Username, (HashedPassword hashed_password, User user)> users;
        private IDictionary<MachineIDToken, (AuthToken authtoken, Username username)> tokenized_users;
        private readonly object lock_object = new object();

        private IDateTimeProvider datetime_provider;

        public VolatileUserService(IDateTimeProvider datetime_provider)
        {
            this.current_id = 0;
            this.datetime_provider = datetime_provider;
            this.users = new Dictionary<Username, (HashedPassword password, User user)>(new UsernameComparer());
            this.tokenized_users = new Dictionary<MachineIDToken, (AuthToken authtoken, Username username)>(new MachineIDTokenComparer());
        }

        private bool ValidateCredentials(Username username, string password)
        {
            return users.ContainsKey(username) && users[username].hashed_password.Verify(password);
        }

        private bool ValidateCredentials(TokenCredentials credentials)
        {
            if (!tokenized_users.ContainsKey(credentials.IDToken))
                return false;

            if (credentials.IDToken.IsExpired())
            {
                tokenized_users.Remove(credentials.IDToken);
                return false;
            }
            
            if (tokenized_users[credentials.IDToken].authtoken.Matches(credentials.UserAuthToken))
            {
                // TODO: Consider bumping session expiration?
                return true;
            }

            return false;
        }

        public User GetUser(Username username, string password)
        {
            if (ValidateCredentials(username, password))
                return users[username].user;

            return null;
        }

        public User GetUser(TokenCredentials credentials)
        {
            if (ValidateCredentials(credentials))
                return users[tokenized_users[credentials.IDToken].username].user;
            return null;
        }

        public TokenCredentials GenerateTokens(User user)
        {
            MachineIDToken idtoken = MachineIDToken.Generate(this.datetime_provider);
            AuthToken authtoken = AuthToken.Generate();

            tokenized_users.Add(idtoken, (authtoken, user.Username));

            return new TokenCredentials(idtoken, authtoken);
        }

        public void RemoveTokens(MachineIDToken idtoken)
        {
            tokenized_users.Remove(idtoken);
        }

        public User CreateUser(Username username, HashedPassword password, PhoneNumber phone_number)
        {
            User created_user;
            lock (lock_object) { created_user = new User(current_id++, username, phone_number); }
            users.Add(username, (password, created_user));
            return created_user;
        }

        public bool Exists(Username username)
        {
            return users.ContainsKey(username);
        }

        public bool Exists(int user_id)
        {
            return users.Any(tr => tr.Value.user.ID == user_id);
        }
    }
}

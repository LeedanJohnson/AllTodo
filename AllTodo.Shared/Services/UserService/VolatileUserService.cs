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

        private bool ValidateTokens(MachineIDToken idtoken, AuthToken authtoken)
        {
            if (!tokenized_users.ContainsKey(idtoken))
                return false;

            if (idtoken.IsExpired())
            {
                tokenized_users.Remove(idtoken);
                return false;
            }
            
            if (tokenized_users[idtoken].authtoken.Matches(authtoken))
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

        public User GetUser(MachineIDToken idtoken, AuthToken authtoken)
        {
            if (ValidateTokens(idtoken, authtoken))
                return users[tokenized_users[idtoken].username].user;
            return null;
        }

        public (MachineIDToken idtoken, AuthToken authtoken) GenerateTokens(User user)
        {
            MachineIDToken idtoken = new MachineIDToken(this.datetime_provider);
            AuthToken authtoken = new AuthToken();

            tokenized_users.Add(idtoken, (authtoken, user.Username));

            return (idtoken, authtoken);
        }

        public void RemoveTokens(string idtoken)
        {
            MachineIDToken IDToken = new MachineIDToken(idtoken, this.datetime_provider);
            tokenized_users.Remove(IDToken);
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

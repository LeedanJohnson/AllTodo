using System;
using System.Collections.Generic;
using System.Text;
using AllTodo.Shared.Models;

namespace AllTodo.Shared.Services
{
    public class VolatileUserService : IUserService
    {
        private int current_id;
        IDictionary<Username, (Password password, User user)> users;
        private readonly object lock_object = new object();

        public VolatileUserService()
        {
            this.current_id = 0;
            users = new Dictionary<Username, (Password password, User user)>();
        }

        public User CreateUser(Username username, Password password, PhoneNumber phone_number)
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

        public User GetUser(Username username)
        {
            return users[username].user;
        }

        public Password GetUserPassword(Username username)
        {
            return users[username].password;
        }
    }
}

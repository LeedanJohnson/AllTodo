using AllTodo.Shared.Models;
using AllTodo.Shared.Models.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.Shared.Services
{
    public interface IUserService
    {
        User GetUser(Username username, string password);
        User GetUser(MachineIDToken idtoken, AuthToken authtoken);
        (MachineIDToken idtoken, AuthToken authtoken) GenerateTokens(User user);
        void RemoveTokens(string idtoken);
        User CreateUser(Username username, HashedPassword password, PhoneNumber phone_number);
        bool Exists(Username username);
        bool Exists(int user_id);
    }
}

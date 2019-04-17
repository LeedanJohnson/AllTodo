using AllTodo.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.Shared.Services
{
    public interface IUserService
    {
        User CreateUser(Username username, Password password, PhoneNumber phone_number);
        User GetUser(Username username);
        Password GetUserPassword(Username username);
        bool Exists(Username username);
    }
}

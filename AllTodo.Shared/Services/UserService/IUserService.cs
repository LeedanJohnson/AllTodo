using AllTodo.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllTodo.Shared.Services
{
    public interface IUserService
    {
        Task<bool> ValidateCredentials(Username username, string password, out User user);
        User CreateUser(Username username, Password password, PhoneNumber phone_number)
    }
}

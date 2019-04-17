using AllTodo.Shared.Models;
using AllTodo.Shared.Models.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllTodo.Shared.Services
{
    public interface IAuthService
    {
        bool ValidateCredentials(Username username, string password, out User user);
        (MachineIDToken idtoken, AuthToken authtoken) GenerateTokens(User user);
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AllTodo.Shared.Services;
using AllTodo.Shared.Models;
using AllTodo.Shared.Models.Primitives;

namespace AllTodo.Server.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        private IUserService user_service;

        public AccountController(IUserService user_service)
        {
            this.user_service = user_service;
        }

        [HttpPost]
        public IActionResult CreateAccount(string username, string password, string phone_number)
        {
            if (username == null || username == string.Empty)
                return BadRequest();
            if (password == null || password == string.Empty)
                return BadRequest();
            if (phone_number == null)
                return BadRequest();

            if (this.user_service.Exists(new Username(username)))
                return BadRequest();

            this.user_service.CreateUser(new Username(username), new HashedPassword(password), new PhoneNumber(phone_number));

            User user = user_service.GetUser(new Username(username), password);

            if (user != null)
            {
                (MachineIDToken idtoken, AuthToken authtoken) tokens = user_service.GenerateTokens(user);
                return Ok(tokens);
            }
            return BadRequest();
        }
    }
}

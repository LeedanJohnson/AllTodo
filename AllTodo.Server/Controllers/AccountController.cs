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
        public IActionResult CreateAccount([FromBody] (string username, string password, string phone_number) data)
        {
            if (data.username == null || data.username == string.Empty)
                return BadRequest();
            if (data.password == null || data.password == string.Empty)
                return BadRequest();
            if (data.phone_number == null)
                return BadRequest();

            if (this.user_service.Exists(new Username(data.username)))
                return BadRequest();

            this.user_service.CreateUser(new Username(data.username), new HashedPassword(data.password), new PhoneNumber(data.phone_number));

            User user = user_service.GetUser(new Username(data.username), data.password);

            if (user != null)
            {
                TokenCredentials tokens = user_service.GenerateTokens(user);
                return Ok(tokens.GetDTO());
            }
            return BadRequest();
        }
    }
}

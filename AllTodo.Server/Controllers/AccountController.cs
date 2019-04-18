using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AllTodo.Shared.Services;
using AllTodo.Shared.Models;
using AllTodo.Shared.Models.Primitives;
using AllTodo.Shared.Models.OperationObjects;

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
        public IActionResult CreateAccount([FromBody] CreateAccountData data)
        {
            if (data.Username == null || data.Username == string.Empty)
                return BadRequest();
            if (data.Password == null || data.Password == string.Empty)
                return BadRequest();
            if (data.PhoneNumber == null)
                return BadRequest();

            if (this.user_service.Exists(new Username(data.Username)))
                return BadRequest();

            this.user_service.CreateUser(new Username(data.Username), new HashedPassword(data.Password), new PhoneNumber(data.PhoneNumber));

            User user = user_service.GetUser(new Username(data.Username), data.Password);

            if (user != null)
            {
                TokenCredentials tokens = user_service.GenerateTokens(user);
                return Ok(tokens.GetDTO());
            }
            return BadRequest();
        }
    }
}

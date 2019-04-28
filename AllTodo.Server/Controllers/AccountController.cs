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
        public IActionResult CreateAccount([FromBody](string username, string password, string phone_number) data)
        {
            var validate_username = Username.Validate(data.username);
            if (!validate_username.success)
                return BadRequest(validate_username.message);

            var validate_password = RawPassword.Validate(data.password);
            if (!validate_password.success)
                return BadRequest(validate_password.message);

            var validate_phone_number = PhoneNumber.Validate(data.phone_number);
            if (!validate_phone_number.success)
                return BadRequest(validate_phone_number.message);

            if (this.user_service.Exists(new Username(data.username)))
                return BadRequest("Username already exists");

            this.user_service.CreateUser(new Username(data.username), new HashedPassword(data.password), new PhoneNumber(data.phone_number));

            User user = user_service.GetUser(new Username(data.username), data.password);

            if (user == null)
                return BadRequest();

            TokenCredentials tokens = user_service.GenerateTokens(user);
            return Created("api/login", tokens.GetDTO());
        }
    }
}

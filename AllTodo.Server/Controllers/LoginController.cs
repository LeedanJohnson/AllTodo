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
    [Route("api/login")]
    public class LoginController : Controller
    {
        public LoginController(IAuthService auth_service, IDateTimeProvider datetime_provider)
        {
            this.auth_service = auth_service;
            this.datetime_provider = datetime_provider;
        }

        private IAuthService auth_service;
        private IDateTimeProvider datetime_provider;

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == null || username == string.Empty)
                return BadRequest();
            if (password == null || password == string.Empty)
                return BadRequest();

            User user = null;
            (MachineIDToken idtoken, AuthToken authtoken) tokens;

            if (this.auth_service.ValidateCredentials(new Username(username), password, out user))
            {
                tokens = auth_service.GenerateTokens(user);
                return Ok(tokens);
            }
            return BadRequest();
        }

        [HttpDelete]
        public IActionResult Logout(string idtoken, string authtoken)
        {
            if (idtoken == null || idtoken == string.Empty)
                return BadRequest();
            if (authtoken == null || authtoken == string.Empty)
                return BadRequest();

            User user = null;
            (MachineIDToken idtoken, AuthToken authtoken) tokens;

            if (this.auth_service.ValidateTokens(idtoken, authtoken))
            {
                tokens = auth_service.RemoveTokens(user);
                return Ok(tokens);
            }
            return BadRequest();
        }
    }
}

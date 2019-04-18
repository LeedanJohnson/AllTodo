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
        public LoginController(IUserService userservice, IDateTimeProvider datetimeprovider)
        {
            this.userservice = userservice;
            this.datetimeprovider = datetimeprovider;
        }

        private IUserService userservice;
        private IDateTimeProvider datetimeprovider;

        [HttpPost]
        public IActionResult Login([FromBody](string username, string password) data)
        {
            if (data.username == null || data.username == string.Empty)
                return BadRequest();
            if (data.password == null || data.password == string.Empty)
                return BadRequest();

            User user = this.userservice.GetUser(new Username(data.username), data.password);

            if (user != null)
            {
                TokenCredentials tokens = userservice.GenerateTokens(user);
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

            User user = this.userservice.GetUser(new MachineIDToken(idtoken, this.datetimeprovider), new AuthToken(authtoken));

            if (user != null)
            {
                userservice.RemoveTokens(idtoken);
                return Ok();
            }
            return BadRequest();
        }
    }
}

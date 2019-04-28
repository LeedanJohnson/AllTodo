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

        [HttpGet]
        public IActionResult Login([FromHeader]string username, [FromHeader]string password)
        {
            var validate_username = Username.Validate(username);
            if (!validate_username.success)
                return BadRequest(validate_username.message);

            var validate_password = RawPassword.Validate(password);
            if (!validate_password.success)
                return BadRequest(validate_password.message);

            User user = this.userservice.GetUser(new Username(username), password);

            if (user == null)
                return Unauthorized();

            TokenCredentials tokens = userservice.GenerateTokens(user);
            return Ok(tokens.GetDTO());
        }

        [HttpDelete]
        public IActionResult Logout([FromHeader]string idtoken, [FromHeader]string authtoken)
        {
            TokenCredentialsDTO credentials_dto = new TokenCredentialsDTO(idtoken, authtoken);
            var credential_validation = credentials_dto.Validate();
            if (!credential_validation.success)
                return BadRequest("Bad Credentials: " + credential_validation.message);

            TokenCredentials credentials = credentials_dto.GetObject(datetimeprovider);

            User user = this.userservice.GetUser(credentials);

            if (user != null)
            {
                userservice.RemoveTokens(credentials.IDToken);
                return Ok();
            }
            return BadRequest();
        }
    }
}

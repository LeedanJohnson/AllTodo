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
    [Route("api/todos")]
    public class TodoController : Controller
    {
        private ITodoService todoservice;
        private IUserService userservice;
        private IDateTimeProvider datetimeprovider;

        public TodoController(IUserService userservice, ITodoService todoservice, IDateTimeProvider datetimeprovider)
        {
            this.userservice = userservice;
            this.todoservice = todoservice;
            this.datetimeprovider = datetimeprovider;
        }

        [HttpGet()]
        public IActionResult GetTodos([FromHeader]string idtoken, [FromHeader]string authtoken)
        {
            TokenCredentialsDTO credentials_dto = new TokenCredentialsDTO(idtoken, authtoken);
            var credential_validation = credentials_dto.Validate();
            if (!credential_validation.success)
                return BadRequest("Bad Credentials: " + credential_validation.message);

            TokenCredentials credentials = credentials_dto.GetObject(datetimeprovider);

            User user = this.userservice.GetUser(credentials);

            if (user == null)
                return Unauthorized();

            return Ok(this.todoservice.GetTodos(user));
        }

        [HttpGet("{id}")]
        public IActionResult GetTodo([FromHeader]string idtoken, [FromHeader]string authtoken, [FromRoute]int id )
        {
            TokenCredentialsDTO credentials_dto = new TokenCredentialsDTO(idtoken, authtoken);
            var credential_validation = credentials_dto.Validate();
            if (!credential_validation.success)
                return BadRequest("Bad Credentials: " + credential_validation.message);

            TokenCredentials credentials = credentials_dto.GetObject(datetimeprovider);

            User user = this.userservice.GetUser(credentials);

            if (user == null)
                return Unauthorized();
            
            return Ok(this.todoservice.GetTodo(id, user));
        }

        [HttpPost]
        public IActionResult CreateTodo([FromHeader]string idtoken, [FromHeader]string authtoken, [FromBody]TodoDTO dto)
        {
            TokenCredentialsDTO credentials_dto = new TokenCredentialsDTO(idtoken, authtoken);
            var credential_validation = credentials_dto.Validate();
            if (!credential_validation.success)
                return BadRequest("Bad Credentials: " + credential_validation.message);

            TokenCredentials credentials = credentials_dto.GetObject(datetimeprovider);

            // TODO: Replace with DTO validation
            if (dto.Title == null || dto.Title == string.Empty)
                return BadRequest();
            if (dto.Description == null || dto.Description == string.Empty)
                return BadRequest();
            if ((int)dto.State < 0 || (int)dto.State > 2)
                return BadRequest();

            User user = this.userservice.GetUser(credentials);

            if (user == null)
                return Unauthorized();
            
            dto.UserID = user.ID;

            (bool success, string message) validation_result = dto.Validate();
            if (!validation_result.success)
                return BadRequest(validation_result.message);

            (bool success, string message) identifier_validation_result = dto.ValidateIdentifiers(userservice, todoservice);
            if (!identifier_validation_result.success)
                return BadRequest(identifier_validation_result.message);

            Todo todo = this.todoservice.CreateTodo(dto, user);
            return Created($"api/todos/{todo.ID}", todo);
        }

        [HttpPatch()]
        public IActionResult UpdateTodo([FromHeader]string idtoken, [FromHeader]string authtoken, [FromBody]TodoDTO dto)
        {
            TokenCredentialsDTO credentials_dto = new TokenCredentialsDTO(idtoken, authtoken);
            var credential_validation = credentials_dto.Validate();
            if (!credential_validation.success)
                return BadRequest("Bad Credentials: " + credential_validation.message);

            TokenCredentials credentials = credentials_dto.GetObject(datetimeprovider);

            User user = this.userservice.GetUser(credentials);
            if (user == null)
                return Unauthorized();

            if (!this.todoservice.Exists(dto.ID, user))
                return BadRequest();

            TodoDTO new_dto = this.todoservice.GetTodo(dto.ID, user).GetDTO();

            if (dto.Title != null && dto.Title != string.Empty)
                new_dto.Title = dto.Title;
            if (dto.Description != null && dto.Description != string.Empty)
                new_dto.Description = dto.Description;
            if (dto.State > 0)
                new_dto.State = (TodoState)dto.State;

            var validation_result = new_dto.Validate();
            if (!validation_result.success)
                return BadRequest(validation_result.message);

            var identifier_validation_result = new_dto.ValidateIdentifiers(userservice, todoservice);
            if (!identifier_validation_result.success)
                return BadRequest(identifier_validation_result.message);

            Todo updated_todo = this.todoservice.UpdateTodo(new_dto, user);

            return Ok(updated_todo);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTodo([FromHeader]string idtoken, [FromHeader]string authtoken, [FromRoute]int id)
        {
            TokenCredentialsDTO credentials_dto = new TokenCredentialsDTO(idtoken, authtoken);
            var credential_validation = credentials_dto.Validate();
            if (!credential_validation.success)
                return BadRequest("Bad Credentials: " + credential_validation.message);

            TokenCredentials credentials = credentials_dto.GetObject(datetimeprovider);

            User user = this.userservice.GetUser(credentials);
            if (user == null)
                return Unauthorized();

            this.todoservice.RemoveTodo(id, user);
            return NoContent();
        }
    }
}

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
        public IActionResult GetTodos(string idtoken, string authtoken)
        {
            if (idtoken == null || idtoken == string.Empty)
                return BadRequest();
            if (authtoken == null || authtoken == string.Empty)
                return BadRequest();

            User user = this.userservice.GetUser(new MachineIDToken(idtoken, this.datetimeprovider), new AuthToken(authtoken));

            if (user != null)
                return Ok(this.todoservice.GetTodos(user));

            return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult GetTodo([FromBody](TokenCredentialsDTO credentials, int id) data)
        {
            if (data.credentials.IDToken == null || data.credentials.IDToken == string.Empty)
                return BadRequest();
            if (data.credentials.AuthToken == null || data.credentials.AuthToken == string.Empty)
                return BadRequest();

            User user = this.userservice.GetUser(new MachineIDToken(data.credentials.IDToken, this.datetimeprovider), new AuthToken(data.credentials.AuthToken));

            if (user != null)
                return Ok(this.todoservice.GetTodo(data.id, user));

            return NotFound();
        }

        [HttpPost]
        public IActionResult CreateTodo([FromBody](TokenCredentialsDTO credentials, TodoDTO dto) data)
        {
            if (data.credentials.IDToken == null || data.credentials.IDToken == string.Empty)
                return BadRequest();
            if (data.credentials.AuthToken == null || data.credentials.AuthToken == string.Empty)
                return BadRequest();
            if (data.dto.Title == null || data.dto.Title == string.Empty)
                return BadRequest();
            if (data.dto.Description == null || data.dto.Description == string.Empty)
                return BadRequest();
            if ((int)data.dto.State < 0 || (int)data.dto.State > 2)
                return BadRequest();

            User user = this.userservice.GetUser(new MachineIDToken(data.credentials.IDToken, this.datetimeprovider), new AuthToken(data.credentials.AuthToken));

            if (user != null)
            {
                data.dto.UserID = user.ID;

                (bool success, string message) validation_result = data.dto.Validate(this.userservice);

                if (!validation_result.success)
                    return BadRequest(validation_result.message);

                return Ok(this.todoservice.CreateTodo(data.dto, user));
            }

            return BadRequest();
        }

        [HttpPatch()]
        public IActionResult UpdateTodo([FromBody](TokenCredentialsDTO credentials, TodoDTO dto) data)
        {
            if (data.credentials.IDToken == null || data.credentials.IDToken == string.Empty)
                return BadRequest();
            if (data.credentials.AuthToken == null || data.credentials.AuthToken == string.Empty)
                return BadRequest();

            User user = this.userservice.GetUser(new MachineIDToken(data.credentials.IDToken, this.datetimeprovider), new AuthToken(data.credentials.AuthToken));
            if (user != null)
            {
                if (!this.todoservice.Exists(data.dto.ID, user))
                    return BadRequest();

                TodoDTO dto = this.todoservice.GetTodo(data.dto.ID, user).GetDTO();

                if (data.dto.Title != null && data.dto.Title != string.Empty)
                    dto.Title = data.dto.Title;
                if (data.dto.Description != null && data.dto.Description != string.Empty)
                    dto.Description = data.dto.Description;
                if (data.dto.State > 0)
                    dto.State = (TodoState)data.dto.State;

                (bool success, string message) validation_result = dto.Validate(this.userservice);

                if (!validation_result.success)
                    return BadRequest(validation_result.message);

                Todo updated_todo = this.todoservice.UpdateTodo(dto, user);

                return Ok(updated_todo);
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTodo([FromBody](TokenCredentialsDTO credentials, int id) data)
        {
            if (data.credentials.IDToken == null || data.credentials.IDToken == string.Empty)
                return BadRequest();
            if (data.credentials.AuthToken == null || data.credentials.AuthToken == string.Empty)
                return BadRequest();

            User user = this.userservice.GetUser(new MachineIDToken(data.credentials.IDToken, this.datetimeprovider), new AuthToken(data.credentials.AuthToken));
            if (user != null)
            {
                this.todoservice.RemoveTodo(data.id, user);
                return Ok();
            }
            return BadRequest();
        }
    }
}

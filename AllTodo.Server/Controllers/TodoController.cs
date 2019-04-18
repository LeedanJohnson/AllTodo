using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AllTodo.Shared.Services;
using AllTodo.Shared.Models;
using AllTodo.Shared.Models.Primitives;
using AllTodo.Shared.Models.DTOs;

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
        public IActionResult GetTodo(string idtoken, string authtoken, int id)
        {
            if (idtoken == null || idtoken == string.Empty)
                return BadRequest();
            if (authtoken == null || authtoken == string.Empty)
                return BadRequest();

            User user = this.userservice.GetUser(new MachineIDToken(idtoken, this.datetimeprovider), new AuthToken(authtoken));

            if (user != null)
                return Ok(this.todoservice.GetTodo(id, user));

            return NotFound();
        }

        [HttpPost]
        public IActionResult CreateTodo(string idtoken, string authtoken, string title, string description, int state = 0)
        {
            if (idtoken == null || idtoken == string.Empty)
                return BadRequest();
            if (authtoken == null || authtoken == string.Empty)
                return BadRequest();
            if (title == null || title == string.Empty)
                return BadRequest();
            if (description == null || description == string.Empty)
                return BadRequest();
            if (state < 0 || state > 2)
                return BadRequest();

            User user = this.userservice.GetUser(new MachineIDToken(idtoken, this.datetimeprovider), new AuthToken(authtoken));

            if (user != null)
            {
                TodoDTO dto = new TodoDTO();
                dto.UserID = user.ID;
                dto.Title = title;
                dto.Description = description;
                dto.State = (TodoState)state;

                (bool success, string message) validation_result = dto.Validate(this.userservice);

                if (!validation_result.success)
                    return BadRequest(validation_result.message);

                return Ok(this.todoservice.CreateTodo(dto, user));
            }

            return BadRequest();
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateTodo(string idtoken, string authtoken, int id, string title, string description, int state = -1)
        {
            if (idtoken == null || idtoken == string.Empty)
                return BadRequest();
            if (authtoken == null || authtoken == string.Empty)
                return BadRequest();

            User user = this.userservice.GetUser(new MachineIDToken(idtoken, this.datetimeprovider), new AuthToken(authtoken));
            if (user != null)
            {
                if (!this.todoservice.Exists(id, user))
                    return BadRequest();

                TodoDTO dto = this.todoservice.GetTodo(id, user).GetDTO();

                if (title != null && title != string.Empty)
                    dto.Title = title;
                if (description != null && description != string.Empty)
                    dto.Description = description;
                if (state < 0)
                    dto.State = (TodoState)state;

                (bool success, string message) validation_result = dto.Validate(this.userservice);

                if (!validation_result.success)
                    return BadRequest(validation_result.message);

                Todo updated_todo = this.todoservice.UpdateTodo(dto, user);

                return Ok(updated_todo);
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTodo(string idtoken, string authtoken, int id)
        {
            if (idtoken == null || idtoken == string.Empty)
                return BadRequest();
            if (authtoken == null || authtoken == string.Empty)
                return BadRequest();

            User user = this.userservice.GetUser(new MachineIDToken(idtoken, this.datetimeprovider), new AuthToken(authtoken));
            if (user != null)
            {
                this.todoservice.RemoveTodo(id, user);
                return Ok();
            }
            return BadRequest();
        }
    }
}

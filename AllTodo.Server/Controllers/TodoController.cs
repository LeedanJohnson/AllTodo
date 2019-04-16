using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AllTodo.Shared.Services;
using AllTodo.Shared.Models;

namespace AllTodo.Server.Controllers
{
    [Route("api/todos")]
    public class TodoController: Controller
    {
        public TodoController(ITodoService todo_service)
        {
            this.todo_service = todo_service;
        }

        private ITodoService todo_service;

        [HttpGet()]
        public IActionResult GetTodos()
        {
            return Ok(this.todo_service.GetTodos());
        }

        [HttpGet("{id}")]
        public IActionResult GetTodo(int id)
        {
            Todo candidate = this.todo_service.GetTodo(id);
            if (candidate == null)
                return NotFound();
            return Ok(candidate);
        }

        [HttpPost]
        public IActionResult CreateTodo(string title, string description, int state = 0)
        {
            if (title == null || title == string.Empty)
                return BadRequest();
            if (description == null || description == string.Empty)
                return BadRequest();
            if (state < 0 || state > 2)
                return BadRequest();

            Todo new_todo = this.todo_service.CreateTodo(title, description, (TodoState)state);

            return Ok(new_todo);
        }
    }
}

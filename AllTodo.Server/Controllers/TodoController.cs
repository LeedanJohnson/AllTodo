using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllTodo.Server.Controllers
{
    [Route("api/todos")]
    public class TodoController: Controller
    {
        [HttpGet()]
        public JsonResult GetTodos()
        {
            return new JsonResult(new List<Object>()
            {
                new {id = 1, Name = "Do this assignment"},
                new {id = 2, Name = "Celebrate"}
            });
        }
    }
}

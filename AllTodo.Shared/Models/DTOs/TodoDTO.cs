using AllTodo.Shared.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.Shared.Models.DTOs
{
    public class TodoDTO
    {
        public int ID { get; set; } = 0;
        public int UserID { get; set; } = -1;
        public string Title { get; set; }
        public string Description { get; set; }
        public TodoState State { get; set; }

        public (bool success, string message) Validate(IUserService userservice)
        {
            return Todo.Validate(this, userservice);
        }
    }
}

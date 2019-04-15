using AllTodo.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.Shared.Services
{
    interface ITodoService
    {
        Todo CreateTodo(string title, string description, TodoState state = TodoState.NOT_STARTED);

        Todo UpdateTodo(int id, string title, string description, TodoState state);
        Todo UpdateTodoState(int id, TodoState state);

        void RemoveTodo(Todo todo);
        void RemoveTodo(int id);

        IReadOnlyList<Todo> GetTodos();
    }
}

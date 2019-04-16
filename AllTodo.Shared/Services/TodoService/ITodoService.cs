using AllTodo.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.Shared.Services
{
    public interface ITodoService
    {
        Todo CreateTodo(string title, string description, TodoState state);

        Todo UpdateTodo(Todo todo, string title, string description, TodoState state);
        Todo UpdateTodo(int id, string title, string description, TodoState state);
        Todo UpdateTodoState(Todo todo, TodoState state);
        Todo UpdateTodoState(int id, TodoState state);

        void RemoveTodo(Todo todo);
        void RemoveTodo(int todo_id);

        IReadOnlyList<Todo> GetTodos();
        Todo GetTodo(int id);
        Todo GetTodo(Todo todo);

        bool Exists(int id);
        bool Exists(Todo todo);
    }
}

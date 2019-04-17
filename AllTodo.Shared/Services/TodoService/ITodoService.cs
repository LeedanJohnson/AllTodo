using AllTodo.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.Shared.Services
{
    public interface ITodoService
    {
        Todo CreateTodo(User user, string title, string description, TodoState state);

        Todo UpdateTodo(User user, Todo todo, string title, string description, TodoState state);
        Todo UpdateTodo(User user, int id, string title, string description, TodoState state);
        Todo UpdateTodoState(User user, Todo todo, TodoState state);
        Todo UpdateTodoState(User user, int id, TodoState state);

        void RemoveTodo(User user, Todo todo);
        void RemoveTodo(User user, int todo_id);

        IReadOnlyList<Todo> GetTodos(User user);
        Todo GetTodo(User user, int id);
        Todo GetTodo(User user, Todo todo);

        bool Exists(User user, int id);
        bool Exists(User user, Todo todo);
    }
}

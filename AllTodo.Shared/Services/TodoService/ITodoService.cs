using AllTodo.Shared.Models;
using AllTodo.Shared.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.Shared.Services
{
    public interface ITodoService
    {
        Todo CreateTodo(TodoDTO todo_dto, User user);
        Todo UpdateTodo(TodoDTO todo_dto, User user);
        Todo UpdateTodoState(Todo todo, TodoState state, User user);
        Todo UpdateTodoState(int todo_id, TodoState state, User user);
        void RemoveTodo(Todo todo, User user);
        void RemoveTodo(int todo_id, User user);
        IReadOnlyList<Todo> GetTodos(User user);
        Todo GetTodo(int id, User user);
        Todo GetTodo(Todo todo, User user);
        bool Exists(int id, User user);
        bool Exists(Todo todo, User user);
    }
}

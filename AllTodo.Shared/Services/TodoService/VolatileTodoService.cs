using AllTodo.Shared.Models;
using AllTodo.Shared.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllTodo.Shared.Services
{
    public class VolatileTodoService : ITodoService
    {
        public VolatileTodoService(IUserService userservice)
        {
            this.current_id = 0;
            this.userservice = userservice;
            todos = new List<Todo>();
        }

        private IUserService userservice;
        private int current_id;
        private readonly object lock_object = new object();

        List<Todo> todos;

        public Todo CreateTodo(TodoDTO todo_dto, User user)
        {
            lock (lock_object) { todo_dto.ID = current_id++; }
            Todo created_todo;
            created_todo = new Todo(todo_dto, userservice);
            todos.Add(created_todo);
            return created_todo;
        }

        public Todo UpdateTodo(TodoDTO todo_dto, User user)
        {
            if (!this.Exists(todo_dto.ID, user))
                return null;

            if (!todo_dto.Validate(userservice).success)
                return null;

            Todo updated = new Todo(todo_dto, userservice);

            RemoveTodo(todo_dto.ID, user);
            this.todos.Add(updated);
            return updated;
        }

        public Todo UpdateTodoState(Todo todo, TodoState state, User user)
        {
            if (!this.Exists(todo, user))
                return null;

            TodoDTO dto = todo.GetDTO();
            dto.State = state;

            if (!dto.Validate(userservice).success)
                return null;

            Todo updated = new Todo(dto, userservice);

            RemoveTodo(todo, user);
            this.todos.Add(updated);
            return updated;
        }

        public Todo UpdateTodoState(int todo_id, TodoState state, User user)
        {
            if (!IsAuthorized(todo_id, user))
                return null;

            Todo to_update = this.todos.SingleOrDefault(t => t.ID == todo_id);
            return UpdateTodoState(to_update, state, user);
        }

        public void RemoveTodo(Todo todo, User user)
        {
            if (!IsAuthorized(todo, user))
                return;

            this.todos.Remove(todo);
        }

        public void RemoveTodo(int todo_id, User user)
        {
            if (!IsAuthorized(todo_id, user))
                return;

            todos.Remove(todos.SingleOrDefault(t => t.ID == todo_id));
        }

        public IReadOnlyList<Todo> GetTodos(User user)
        {
            return new List<Todo>(todos.Where(t => t.UserID == user.ID));
        }

        public Todo GetTodo(int id, User user)
        {
            if (!IsAuthorized(id, user))
                return null;

            return todos.SingleOrDefault(t => t.ID == id);
        }

        public Todo GetTodo(Todo todo, User user)
        {
            if (!IsAuthorized(todo, user))
                return null;

            return todos.SingleOrDefault(t => t.ID == todo.ID);
        }

        public bool Exists(int id, User user)
        {
            if (!IsAuthorized(id, user))
                return false;

            return todos.Exists(t => t.ID == id);
        }

        public bool Exists(Todo todo, User user)
        {
            if (!IsAuthorized(todo, user))
                return false;

            return todos.Exists(t => t.ID == todo.ID);
        }

        public bool IsAuthorized(Todo todo, User user)
        {
            return todo.UserID == user.ID;
        }

        public bool IsAuthorized(int todo_id, User user)
        {
            Todo todo = todos.SingleOrDefault(t => t.ID == todo_id);
            return todo.UserID == user.ID;
        }
    }
}

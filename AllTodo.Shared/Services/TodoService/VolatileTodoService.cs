using AllTodo.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllTodo.Shared.Services
{
    public class VolatileTodoService : ITodoService
    {
        public VolatileTodoService(IAuthService authservice)
        {
            this.current_id = 0;
            this.authservice = authservice;
        }

        private IAuthService authservice;
        private int current_id;
        private readonly object lock_object = new object();

        public Todo CreateTodo(User user, string title, string description, TodoState state)
        {
            Todo created_todo;
            lock (lock_object) { created_todo = new Todo(current_id++, title, description, state); }
            todos.Add(created_todo);
            return created_todo;
        }

        public Todo UpdateTodo(User user, Todo todo, string title, string description, TodoState state)
        {
            if (!this.Exists(user, todo))
                return null;

            Todo updated = new Todo(todo.ID, title, description, state);

            RemoveTodo(user, todo);
            this.todos.Add(updated);
            return updated;
        }

        public Todo UpdateTodo(User user, int id, string title, string description, TodoState state)
        {
            if (!this.Exists(user, id))
                return null;

            Todo updated = new Todo(id, title, description, state);

            RemoveTodo(user, id);
            this.todos.Add(updated);
            return updated;
        }

        public Todo UpdateTodoState(User user, Todo todo, TodoState state)
        {
            if (!this.Exists(user, todo))
                return null;

            Todo updated = new Todo(todo.ID, todo.Title, todo.Description, state);

            RemoveTodo(user, todo);
            this.todos.Add(updated);
            return updated;
        }

        public Todo UpdateTodoState(User user, int id, TodoState state)
        {
            Todo to_update = this.todos.SingleOrDefault(t => t.ID == id);

            if (to_update == null)
                return null;

            return this.UpdateTodoState(user, to_update, state);
        }

        public void RemoveTodo(User user, Todo todo)
        {
            this.todos.Remove(todo);
        }

        public void RemoveTodo(User user, int todo_id)
        {
            todos.Remove(todos.SingleOrDefault(t => t.ID == todo_id));
        }

        public IReadOnlyList<Todo> GetTodos(User user)
        {
            return this.todos;
        }

        public Todo GetTodo(User user, int id)
        {
            return todos.SingleOrDefault(t => t.ID == id);
        }

        public Todo GetTodo(User user, Todo todo)
        {
            return todos.SingleOrDefault(t => t.ID == todo.ID);
        }

        public bool Exists(User user, int id)
        {
            return todos.Exists(t => t.ID == id);
        }

        public bool Exists(User user, Todo todo)
        {
            return todos.Exists(t => t.ID == todo.ID);
        }
    }
}

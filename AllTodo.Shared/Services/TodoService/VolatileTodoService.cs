using AllTodo.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllTodo.Shared.Services
{
    public class VolatileTodoService : ITodoService
    {
        public VolatileTodoService()
        {
            this.current_id = 0;
            this.todos = new List<Todo>();
        }

        private int current_id;
        private List<Todo> todos;
        private readonly object lock_object = new object();

        public Todo CreateTodo(string title, string description, TodoState state)
        {
            Todo created_todo;
            lock (lock_object) { created_todo = new Todo(current_id++, title, description, state); }
            todos.Add(created_todo);
            return created_todo;
        }

        public Todo UpdateTodo(Todo todo, string title, string description, TodoState state)
        {
            if (!this.Exists(todo))
                return null;

            Todo updated = new Todo(todo.ID, title, description, state);

            RemoveTodo(todo);
            this.todos.Add(updated);
            return updated;
        }

        public Todo UpdateTodo(int id, string title, string description, TodoState state)
        {
            if (!this.Exists(id))
                return null;

            Todo updated = new Todo(id, title, description, state);

            RemoveTodo(id);
            this.todos.Add(updated);
            return updated;
        }

        public Todo UpdateTodoState(Todo todo, TodoState state)
        {
            if (!this.Exists(todo))
                return null;

            Todo updated = new Todo(todo.ID, todo.Title, todo.Description, state);

            RemoveTodo(todo);
            this.todos.Add(updated);
            return updated;
        }

        public Todo UpdateTodoState(int id, TodoState state)
        {
            Todo to_update = this.todos.SingleOrDefault(t => t.ID == id);

            if (to_update == null)
                return null;

            return this.UpdateTodoState(to_update, state);
        }

        public void RemoveTodo(Todo todo)
        {
            this.todos.Remove(todo);
        }

        public void RemoveTodo(int todo_id)
        {
            todos.Remove(todos.SingleOrDefault(t => t.ID == todo_id));
        }

        public IReadOnlyList<Todo> GetTodos()
        {
            return this.todos;
        }

        public Todo GetTodo(int id)
        {
            return todos.SingleOrDefault(t => t.ID == id);
        }

        public Todo GetTodo(Todo todo)
        {
            return todos.SingleOrDefault(t => t.ID == todo.ID);
        }

        public bool Exists(int id)
        {
            return todos.Exists(t => t.ID == id);
        }

        public bool Exists(Todo todo)
        {
            return todos.Exists(t => t.ID == todo.ID);
        }
    }
}

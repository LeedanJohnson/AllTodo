using AllTodo.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllTodo.Shared.Services
{
    class VolatileTodoService : ITodoService
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
            Todo updated = new Todo(todo.id, title, description, state);

            this.todos.Remove(todo);
            this.todos.Add(updated);
            return updated;
        }

        public Todo UpdateTodo(int id, string title, string description, TodoState state)
        {
            return this.UpdateTodo(this.todos.Find(t => t.id == id), title, description, state);
        }

        public void RemoveTodo(Todo todo)
        {
            this.todos.Remove(todo);
        }

        public void RemoveTodo(int todo_id)
        {
            todos.Remove(todos.Single(t => t.id == todo_id));
        }

        public IReadOnlyList<Todo> GetTodos()
        {
            return this.todos;
        }

        public Todo GetTodo(int id)
        {
            return todos.Single(t => t.id == id);
        }

        public Todo GetTodo(Todo todo)
        {
            return todos.Single(t => t.id == todo.id);
        }

        public bool Exists(int id)
        {
            return todos.Exists(t => t.id == id);
        }

        public bool Exists(Todo todo)
        {
            return todos.Exists(t => t.id == todo.id);
        }
    }
}

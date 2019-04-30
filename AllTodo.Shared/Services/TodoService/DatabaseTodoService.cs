using System;
using System.Collections.Generic;
using System.Text;
using AllTodo.Shared.Models;
using MySql.Data.MySqlClient;

namespace AllTodo.Shared.Services.TodoService
{
    public class DatabaseTodoService : ITodoService
    {
        IDatabaseService database_service;

        public DatabaseTodoService(IDatabaseService database_service)
        {
            string query = "CREATE TABLE IF NOT EXISTS Todos (" +
                                "id INT NOT NULL AUTO_INCREMENT PRIMARY KEY," +
                                "user_id INT NOT NULL," +
                                "title VARCHAR(50) NOT NULL," +
                                "description VARCHAR(400) NOT NULL," +
                                "state INT NOT NULL);";

            this.database_service = database_service;

            var open_result = database_service.OpenConnection();
            if (!open_result.success)
                return;

            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = query;
            cmd.Connection = database_service.Connection;
            cmd.ExecuteNonQuery();
            database_service.CloseConnection();
        }

        public Todo CreateTodo(TodoDTO todo_dto, User user)
        {
            throw new NotImplementedException();
        }

        public bool Exists(int id, User user)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Todo todo, User user)
        {
            throw new NotImplementedException();
        }

        public Todo GetTodo(int id, User user)
        {
            throw new NotImplementedException();
        }

        public Todo GetTodo(Todo todo, User user)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Todo> GetTodos(User user)
        {
            throw new NotImplementedException();
        }

        public void RemoveTodo(Todo todo, User user)
        {
            throw new NotImplementedException();
        }

        public void RemoveTodo(int todo_id, User user)
        {
            throw new NotImplementedException();
        }

        public Todo UpdateTodo(TodoDTO todo_dto, User user)
        {
            throw new NotImplementedException();
        }

        public Todo UpdateTodoState(Todo todo, TodoState state, User user)
        {
            throw new NotImplementedException();
        }

        public Todo UpdateTodoState(int todo_id, TodoState state, User user)
        {
            throw new NotImplementedException();
        }
    }
}

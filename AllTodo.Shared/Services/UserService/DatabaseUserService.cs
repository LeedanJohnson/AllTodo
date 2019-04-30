using AllTodo.Shared.Models;
using AllTodo.Shared.Models.Primitives;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.Shared.Services.UserService
{
    public class DatabaseUserService : IUserService
    {
        IDatabaseService database_service;

        public DatabaseUserService(IDatabaseService database_service)
        {

            this.database_service = database_service;

            var open_result = database_service.OpenConnection();
            if (!open_result.success)
                return;

            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS Users (" +
                                "id INT NOT NULL AUTO_INCREMENT PRIMARY KEY," +
                                "username VARCHAR(50) NOT NULL," +
                                "phone_number VARCHAR(11) NOT_NULL" +
                                "password_hash VARCHAR(200) NOT NULL);";
            cmd.Connection = database_service.Connection;
            cmd.ExecuteNonQuery();
            
            cmd = new MySqlCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS Tokens (" +
                                "id INT NOT NULL AUTO_INCREMENT PRIMARY KEY," +
                                "machinetoken VARCHAR(50) NOT NULL," +
                                "authtoken VARCHAR(200) NOT NULL," +
                                "expiration DATETIME);";
            cmd.Connection = database_service.Connection;
            cmd.ExecuteNonQuery();

            database_service.CloseConnection();
        }

        public User CreateUser(Username username, HashedPassword password, PhoneNumber phone_number)
        {
            var open_result = database_service.OpenConnection();
            if (!open_result.success)
                return null;
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "INSERT INTO Users(username, phone_number, password_hash)" +
                $"VALUES('{username.Value}', '{phone_number.Value}', {password.Hash});";
            cmd.Connection = database_service.Connection;
            cmd.ExecuteNonQuery();

            cmd = new MySqlCommand();
            cmd.CommandText = "SELECT LAST_INSERT_ID();";
            cmd.Connection = database_service.Connection;
            var Lastid = cmd.ExecuteReader();
            int id = Lastid.GetInt32(0);

            return new User(id, username, phone_number);
        }

        public bool Exists(Username username)
        {
            throw new NotImplementedException();
        }

        public bool Exists(int user_id)
        {
            throw new NotImplementedException();
        }

        public TokenCredentials GenerateTokens(User user)
        {
            throw new NotImplementedException();
        }

        public User GetUser(Username username, string password)
        {
            throw new NotImplementedException();
        }

        public User GetUser(TokenCredentials credentials)
        {
            throw new NotImplementedException();
        }

        public void RemoveTokens(MachineIDToken idtoken)
        {
            throw new NotImplementedException();
        }
    }
}

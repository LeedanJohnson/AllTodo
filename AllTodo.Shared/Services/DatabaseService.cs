using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.Shared.Services
{
    public class DatabaseService : IDatabaseService
    {
        public MySqlConnection connection;
        public MySqlConnection Connection { get { return this.connection; } }
        public bool connection_ok = false;
        public bool ConnectionOK { get { return this.connection_ok; } }
        public bool connection_active = false;
        public bool ConnectionActive { get { return this.connection_active; } }

        //Constructor
        public DatabaseService(string server, string database, string uid, string password)
        {
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            this.connection = new MySqlConnection(connectionString);
            OpenConnection();
            CloseConnection();
        }

        public (bool success, string message) OpenConnection()
        {
            try
            {
                Connection.Open();
                this.connection_ok = true;
                this.connection_active = true;
                return (true, "Connection successful");
            }
            catch (MySqlException ex)
            {
                this.connection_ok = false;
                this.connection_active = false;
                switch (ex.Number)
                {
                    case 0:
                        return (false, "Cannot connect to server.  Contact administrator");

                    case 1045:
                        return (false, "Invalid username/password, please try again");
                }
                return (false, $"Unknown error in connecting to database. Error number: {ex.Number}, Error message: {ex.Message}");
            }
        }

        public (bool success, string message) CloseConnection()
        {
            this.connection_active = false;
            try
            {
                Connection.Close();
                return (true, "Connection terminated successfully");
            }
            catch (MySqlException ex)
            {
                this.connection_ok = false;
                return (false, $"Error in closing database connection. Error number: {ex.Number}, Error message: {ex.Message}");
            }
        }
    }
}

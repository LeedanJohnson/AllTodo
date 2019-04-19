using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.Shared.Services
{
    class DataProvider
    {
        private MySqlConnection connection;

        private DataProvider(string server_ip, string database_name, string username, string password)
        {
            string connstring = string.Format($"Server={server_ip}; database={database_name}; UID={username}; password={password}");
            connection = new MySqlConnection(connstring);
            connection.Open();
        }

        ~DataProvider()
        {
            connection.Close();
        }
                
        public MySqlDataReader Query(string query)
        {
            MySqlCommand cmd = new MySqlCommand(query, this.connection);
            return cmd.ExecuteReader();
        }
    }
}
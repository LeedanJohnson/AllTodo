using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.Shared.Services
{
    public interface IDatabaseService
    {
        MySqlConnection Connection { get; }
        bool ConnectionOK { get; }
        bool ConnectionActive { get; }

        (bool success, string message) OpenConnection();

        (bool success, string message) CloseConnection();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.Shared.Models.OperationObjects
{
    public class LoginData
    {
        public string Username;
        public string Password;

        public LoginData(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public LoginData()
        {
            this.Username = null;
            this.Password = null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.Shared.Models.OperationObjects
{
    public class CreateAccountData
    {
        public string Username;
        public string Password;
        public string PhoneNumber;

        public CreateAccountData(string username, string password, string phone_number)
        {
            this.Username = username;
            this.Password = password;
            this.PhoneNumber = phone_number;
        }

        public CreateAccountData()
        {
            this.Username = null;
            this.Password = null;
            this.PhoneNumber = null;
        }
    }
}

using AllTodo.Shared.Models.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.Shared.Models
{

    public class UserDTO
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class User
    {
        public User(int id, Username username, PhoneNumber phone_number)
        {
            this.id = id;
            this.username = username;
            this.phone_number = phone_number;
        }

        private readonly int id;
        public int ID { get { return this.id; } }

        private readonly Username username;
        public Username Username { get { return username; } }

        private readonly PhoneNumber phone_number;
        public PhoneNumber PhoneNumber { get { return phone_number; } }
    }
}

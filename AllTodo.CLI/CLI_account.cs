using AllTodo.Shared.Models;
using AllTodo.Shared.Models.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.CLI
{
    class CLI_account
    {
        public static void Call(APIClient client, string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Error: improper usage. (--help for usage)");
                return;
            }
            switch (args[0])
            {
                case "--help":
                case "-h":
                    PrintUsage();
                    break;
                case "login":
                    Login(client, ArrayUtils.RemoveFirst(args));
                    break;
                case "create":
                    Create(client, ArrayUtils.RemoveFirst(args));
                    break;

                default:
                    Console.WriteLine("Error: improper usage. (--help for usage)");
                    break;
            }
        }

        static void PrintUsage()
        {
            Console.WriteLine("Usage:\n" +
                                    "\t-h or --help: Displays help and usage information.\n" +
                                    "\tlogin: Used to login.\n" +
                                    "\tcreate: Create an account.\n" );
        }
        
        static void Login(APIClient client, string[] args)
        {
            string username, password;

            Console.WriteLine("Enter Username: ");
            username = Console.ReadLine();
            var username_validation = Username.Validate(username);
            while (!username_validation.success)
            {
                Console.WriteLine($"Invalid Username: {username_validation.message}");
                Console.WriteLine("Enter Username: ");
                username = Console.ReadLine();
                username_validation = Username.Validate(username);
            }

            Console.WriteLine("Enter Password: ");
            password = Console.ReadLine();
            var password_validation = RawPassword.Validate(password);
            while (!password_validation.success)
            {
                Console.WriteLine($"Invalid Password: {password_validation.message}");
                Console.WriteLine("Enter Password: ");
                password = Console.ReadLine();
                password_validation = RawPassword.Validate(password);
            }

            var result = client.Login(username, password);

            Console.WriteLine(result.message);
        }

        static void Create(APIClient client, string[] args)
        {
            (string username, string password, string phone_number) data;

            Console.WriteLine("Enter New Username: ");
            data.username = Console.ReadLine();
            var username_validation = Username.Validate(data.username);
            while (!username_validation.success)
            {
                Console.WriteLine($"Invalid Username: {username_validation.message}");
                Console.WriteLine("Enter New Username: ");
                data.username = Console.ReadLine();
                username_validation = Username.Validate(data.username);
            }

            Console.WriteLine("Enter New Password: ");
            data.password = Console.ReadLine();
            var password_validation = RawPassword.Validate(data.password);
            while (!password_validation.success)
            {
                Console.WriteLine($"Invalid Password: {password_validation.message}");
                Console.WriteLine("Enter New Password: ");
                data.password = Console.ReadLine();
                password_validation = RawPassword.Validate(data.password);
            }

            Console.WriteLine("Enter New Phone Number: ");
            data.phone_number = Console.ReadLine();
            var phone_number_validation = PhoneNumber.Validate(data.phone_number);
            while (!phone_number_validation.success)
            {
                Console.WriteLine($"Invalid Phone Number: {phone_number_validation.message}");
                Console.WriteLine("Enter New Phone Number: ");
                data.phone_number = Console.ReadLine();
                phone_number_validation = PhoneNumber.Validate(data.phone_number);
            }           

            var result = client.Post("api/account", data);

            if (result.response_code == System.Net.HttpStatusCode.Created)
                return;

            Console.WriteLine($"There was an error with your request: Code: {result.response_code} Content:{result.jsonstring}");
            return;
        }
    }
}

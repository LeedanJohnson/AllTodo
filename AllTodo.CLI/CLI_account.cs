using AllTodo.Shared.Models.OperationObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.CLI
{
    class CLI_account
    {
        public static void Call(string[] args)
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
                    Login(ArrayUtils.RemoveFirst(args));
                    break;
                case "create":
                    Create(ArrayUtils.RemoveFirst(args));
                    break;
                case "delete":
                    Delete(ArrayUtils.RemoveFirst(args));
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
                                    "\tcreate: Create an account.\n" +
                                    "\tdelete: Delete an account.\n");
        }
        
        static void Login(string[] args)
        {
            Console.WriteLine("login");
        }

        static void Create(string[] args)
        {
            APIClient client = new APIClient("https://localhost:44343");
            
            CreateAccountData data = new CreateAccountData();

            Console.WriteLine("Enter New Username: ");
            data.Username = Console.ReadLine();

            Console.WriteLine("Enter New Password: ");
            data.Password = Console.ReadLine();

            Console.WriteLine("Enter New Phone Number: ");
            data.PhoneNumber = Console.ReadLine();            

            var result = client.Post("api/account", data);

            Console.Write($"Status: {result.status}, JSON: {result.jsonstring}");
        }

        static void Delete(string[] args)
        {
            Console.WriteLine("Delete Stuff");
        }
    }
}

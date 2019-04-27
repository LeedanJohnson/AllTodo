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
        
        static void Login(string[] args)
        {
            APIClient client = new APIClient("http://localhost:44343");

            (string username, string password) data;

            Console.WriteLine("Enter Username: ");
            data.username = Console.ReadLine();

            Console.WriteLine("Enter Password: ");
            data.password = Console.ReadLine();

            var result = client.Post("api/login", data);

            Console.Write($"Status: {result.status}, JSON: {result.jsonstring}");
        }

        static void Create(string[] args)
        {
            APIClient client = new APIClient("http://localhost:44343");

            (string username, string password, string phone_number) data;

            Console.WriteLine("Enter New Username: ");
            data.username = Console.ReadLine();

            Console.WriteLine("Enter New Password: ");
            data.password = Console.ReadLine();

            Console.WriteLine("Enter New Phone Number: ");
            data.phone_number = Console.ReadLine();            

            var result = client.Post("api/account", data);

            if (result.status != System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"There was an error with your request: {result.jsonstring}");
                return;
            }

            TokenCredentialsDTO credentials = JsonConvert.DeserializeObject<TokenCredentialsDTO>(result.jsonstring);

            Environment.SetEnvironmentVariable("ALLTODO_IDTOKEN", credentials.IDToken, EnvironmentVariableTarget.User);
            Environment.SetEnvironmentVariable("ALLTODO_AUTHTOKEN", credentials.AuthToken, EnvironmentVariableTarget.User);
        }
    }
}

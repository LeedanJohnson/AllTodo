using AllTodo.Shared.Models;
using AllTodo.Shared.Models.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.CLI
{
    class CLI_todo
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
                case "add":
                    Add(ArrayUtils.RemoveFirst(args));
                    break;
                case "remove":
                    Remove(ArrayUtils.RemoveFirst(args));
                    break;
                case "complete":
                    Complete(ArrayUtils.RemoveFirst(args));
                    break;
                case "modify":
                    Modify(ArrayUtils.RemoveFirst(args));
                    break;
                case "list":
                    List(ArrayUtils.RemoveFirst(args));
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
                                    "\tadd: Add a todo.\n" +
                                    "\tremove: Remove a todo.\n" +
                                    "\tupdate: Update a todo state.\n" +
                                    "\tmodify: Modify a todo.\n");
        }

        static void Add(string[] args)
        {
            string idtoken = Environment.GetEnvironmentVariable("ALLTODO_IDTOKEN", EnvironmentVariableTarget.User);
            string authtoken = Environment.GetEnvironmentVariable("ALLTODO_AUTHTOKEN", EnvironmentVariableTarget.User);

            if (idtoken == null || authtoken == null || idtoken == string.Empty || authtoken == string.Empty)
            {
                Console.WriteLine("Please login before continuing");
                return;
            }

            APIClient client = new APIClient("http://localhost:44343");

            TodoDTO dto = new TodoDTO();

            Console.WriteLine("Enter Todo title: ");
            dto.Title = Console.ReadLine();

            Console.WriteLine("Enter Todo description: ");
            dto.Description = Console.ReadLine();

            dto.State = 0;

            var result = client.Post("api/todos", (new TokenCredentialsDTO(idtoken, authtoken), dto));

            if (result.status != System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"There was an error with your request: {result.jsonstring}");
                return;
            }
        }

        static void List(string[] args)
        {
            string idtoken = Environment.GetEnvironmentVariable("ALLTODO_IDTOKEN", EnvironmentVariableTarget.User);
            string authtoken = Environment.GetEnvironmentVariable("ALLTODO_AUTHTOKEN", EnvironmentVariableTarget.User);

            if (idtoken == null || authtoken == null || idtoken == string.Empty || authtoken == string.Empty)
            {
                Console.WriteLine("Please login before continuing");
                return;
            }

            APIClient client = new APIClient("http://localhost:44343");
            var result = client.Get("api/todos", new TokenCredentialsDTO(idtoken, authtoken));

            if (result.status != System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"There was an error with your request: {result.jsonstring}");
                return;
            }

            List<TodoDTO> todos = JsonConvert.DeserializeObject<List<TodoDTO>>(result.jsonstring);

            foreach (TodoDTO todo in todos)
            {
                Console.WriteLine(todo.ToString());
            }
        }

        static void Remove(string[] args)
        {
            string idtoken = Environment.GetEnvironmentVariable("ALLTODO_IDTOKEN", EnvironmentVariableTarget.User);
            string authtoken = Environment.GetEnvironmentVariable("ALLTODO_AUTHTOKEN", EnvironmentVariableTarget.User);

            if (idtoken == null || authtoken == null || idtoken == string.Empty || authtoken == string.Empty)
            {
                Console.WriteLine("Please login before continuing");
                return;
            }
        }

        static void Complete(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Must supply id of todo to mark complete");
                return;
            }

            int id = int.Parse(args[0]);

            string idtoken = Environment.GetEnvironmentVariable("ALLTODO_IDTOKEN", EnvironmentVariableTarget.User);
            string authtoken = Environment.GetEnvironmentVariable("ALLTODO_AUTHTOKEN", EnvironmentVariableTarget.User);

            if (idtoken == null || authtoken == null || idtoken == string.Empty || authtoken == string.Empty)
            {
                Console.WriteLine("Please login before continuing");
                return;
            }

            APIClient client = new APIClient("http://localhost:44343");

            TodoDTO dto = new TodoDTO();

            dto.ID = id;
            dto.State = TodoState.COMPLETED;

            var result = client.Patch("api/todos", (new TokenCredentialsDTO(idtoken, authtoken), dto));

            if (result.status != System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"There was an error with your request: {result.jsonstring}");
                return;
            }
        }

        static void Modify(string[] args)
        {
            string idtoken = Environment.GetEnvironmentVariable("ALLTODO_IDTOKEN", EnvironmentVariableTarget.User);
            string authtoken = Environment.GetEnvironmentVariable("ALLTODO_AUTHTOKEN", EnvironmentVariableTarget.User);

            if (idtoken == null || authtoken == null || idtoken == string.Empty || authtoken == string.Empty)
            {
                Console.WriteLine("Please login before continuing");
                return;
            }
        }
    }
}

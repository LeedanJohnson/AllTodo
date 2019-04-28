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
                case "add":
                    Add(client, ArrayUtils.RemoveFirst(args));
                    break;
                case "remove":
                    Remove(client, ArrayUtils.RemoveFirst(args));
                    break;
                case "complete":
                    Complete(client, ArrayUtils.RemoveFirst(args));
                    break;
                case "modify":
                    Modify(client, ArrayUtils.RemoveFirst(args));
                    break;
                case "list":
                    List(client, ArrayUtils.RemoveFirst(args));
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

        static void Add(APIClient client, string[] args)
        {
            TodoDTO dto = new TodoDTO();

            Console.WriteLine("Enter Todo title: ");
            dto.Title = Console.ReadLine();
            var title_validation = TodoTitle.Validate(dto.Title);
            while (!title_validation.success)
            {
                Console.WriteLine($"Invalid Title: {title_validation.message}");
                Console.WriteLine("Enter Todo title: ");
                dto.Title = Console.ReadLine();

                title_validation = TodoTitle.Validate(dto.Title);
            }

            Console.WriteLine("Enter Todo description: ");
            dto.Description = Console.ReadLine();
            var description_validation = TodoDescription.Validate(dto.Description);
            while (!description_validation.success)
            {
                Console.WriteLine($"Invalid Description: {description_validation.message}");
                Console.WriteLine("Enter Todo description: ");
                dto.Description = Console.ReadLine();

                description_validation = TodoTitle.Validate(dto.Description);
            }

            dto.State = 0;

            var result = client.Post("api/todos", dto);

            if (result.response_code == System.Net.HttpStatusCode.Created)
                return;

            if (result.response_code == System.Net.HttpStatusCode.Unauthorized)
            {
                Console.WriteLine($"It doesn't look like you're logged in! Please log in!");
                return;
            }

            Console.WriteLine($"There was an error with your request: Code: {result.response_code} Content:{result.jsonstring}");
            return;
        }

        static void List(APIClient client, string[] args)
        {
            var result = client.Get("api/todos");

            if (result.response_code == System.Net.HttpStatusCode.OK)
            {
                List<TodoDTO> todos = JsonConvert.DeserializeObject<List<TodoDTO>>(result.jsonstring);
                foreach (TodoDTO todo in todos)
                    Console.WriteLine(todo.ToString());
                return;
            }

            Console.WriteLine($"There was an error with your request: Code: {result.response_code} Content:{result.jsonstring}");
            return;
        }

        static void Complete(APIClient client, string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Must supply id of todo to mark complete");
                return;
            }

            int id = int.Parse(args[0]);

            TodoDTO dto = new TodoDTO();

            dto.ID = id;
            dto.State = TodoState.COMPLETED;

            var result = client.Patch("api/todos", dto);

            if (result.response_code == System.Net.HttpStatusCode.OK)
                return;
            
            Console.WriteLine($"There was an error with your request: Code: {result.response_code} Content:{result.jsonstring}");
            return;
        }

        static void Modify(APIClient client, string[] args)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        static void Remove(APIClient client, string[] args)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }
    }
}

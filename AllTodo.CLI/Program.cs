using System;
using System.Threading.Tasks;

namespace AllTodo.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            APIClient client = new APIClient("https://localhost:44343");

            if (args.Length == 1 && args[0] == "shell")
            {
                Console.WriteLine("Welcome to the AllTodo shell. To exit, type 'exit'");
                string command;
                do
                {
                    Console.Write("AllTodo >> ");
                    command = Console.ReadLine();
                    if (command == "exit")
                        return;
                    CLI_alltodo.Call(client, command.Split(' '));
                } while (true);
            }

            CLI_alltodo.Call(client, args);
        }
    }
}

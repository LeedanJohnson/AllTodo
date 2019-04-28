using System;
using System.Collections.Generic;
using System.Text;

namespace AllTodo.CLI
{
    class CLI_alltodo
    {
        public static void Call(APIClient client, string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Welcome to the All To Do application. (--help for usage)");
                return;
            }
            switch (args[0])
            {
                case "--help":
                case "-h":
                    PrintUsage();
                    break;
                case "account":
                    CLI_account.Call(client, ArrayUtils.RemoveFirst(args));
                    break;
                case "todo":
                    CLI_todo.Call(client, ArrayUtils.RemoveFirst(args));
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
                                    "\taccount: Provides access to account commands.\n");
        }
    }
}

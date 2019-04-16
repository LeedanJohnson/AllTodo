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
                case "update":
                    Update(ArrayUtils.RemoveFirst(args));
                    break;
                case "modify":
                    Modify(ArrayUtils.RemoveFirst(args));
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
            Console.WriteLine("Add Stuff");
        }

        static void Remove(string[] args)
        {
            Console.WriteLine("Remove Stuff");
        }

        static void Update(string[] args)
        {
            Console.WriteLine("Update Stuff");
        }

        static void Modify(string[] args)
        {
            Console.WriteLine("Modify Stuff");
        }
    }
}

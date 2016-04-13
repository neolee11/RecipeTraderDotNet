using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeTraderDotNet.Core.Domain.Repositories;
using RecipeTraderDotNet.Data.Repositories.Memory;

namespace RecipeTraderDotNet.Console
{
    class Program
    {
        private const string CommandPrompt = "> ";

        static void Main(string[] args)
        {
            var systemRunner = new SystemRunner();
            systemRunner.InitializeSystem();
            var command = ShowCommandPrompt();

            while (command != null && command.ToLower() != "quit")
            {
                var output = systemRunner.ProcessCommand(command);
                System.Console.Write(output);
                command = ShowCommandPrompt(2);
            }
        }

        private static string ShowCommandPrompt(int blankLinesBefore = 0)
        {
            for(int i = 0; i < blankLinesBefore; i++)
            {
                System.Console.WriteLine();
            }

            System.Console.Write(CommandPrompt);
            return System.Console.ReadLine();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeTraderDotNet.Console
{
    public class Command
    {
        public CommandType CommandType { get; set; }
        public KeyValuePair<DomainObjectType, string> MainObjPair { get; set; }

        public List<KeyValuePair<string, string>> OptionalCommandPairs { get; set; } = new List<KeyValuePair<string, string>>();

        //public DomainObjectType ObjectType { get; set; }
        //public string Arguments { get; set; }

        public override string ToString()
        {
            var optionalCommand = string.Empty;
            if (OptionalCommandPairs != null && OptionalCommandPairs.Count > 0)
            {
                foreach (var optionalCommandPair in OptionalCommandPairs)
                {
                    optionalCommand += $"{optionalCommandPair.Key} - {optionalCommandPair.Value}\n";
                }
            }
            return $"Command Type : {CommandType}\nMainObjPair : {MainObjPair.Key} - {MainObjPair.Value}\nArguments\n: {optionalCommand}";
        }
    
    }
}

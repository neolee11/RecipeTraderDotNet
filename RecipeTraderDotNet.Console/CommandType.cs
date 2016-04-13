using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeTraderDotNet.Console
{
    public enum CommandType
    {
        Unknown,
        Help,
        Login,
        Show,
        Add,
        Edit,
        Remove,
        Publish,
        Takedown,
        Purchase,
        Review
    }
}

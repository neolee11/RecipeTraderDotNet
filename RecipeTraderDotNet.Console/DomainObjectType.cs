using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeTraderDotNet.Console
{
    public enum DomainObjectType
    {
        Unknown,
        User,
        PrivateRecipe, //private recipe
        PublicRecipe,
        RecipeItem,
        UserAccountBalance,
        SystemStatus
    }
}

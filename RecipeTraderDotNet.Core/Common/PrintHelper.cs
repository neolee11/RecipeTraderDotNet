using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeTraderDotNet.Core.Domain.Recipe;

namespace RecipeTraderDotNet.Core.Common
{
    public static class PrintHelper
    {
        public static string Print(this List<PublicRecipe> publicRecipes)
        {
            if (publicRecipes == null || !publicRecipes.Any()) return String.Empty;

            var output = $"Number of Public Recipes : {publicRecipes.Count}\n\n";

            int count = 0;
            foreach (var publicRecipe in publicRecipes)
            {
                count++;
                output += $"Recipe {count}\n{publicRecipe}\n";
            }

            return output;
        }

        public static string Print(this List<PrivateRecipe> privateRecipes)
        {
            if (privateRecipes == null || !privateRecipes.Any()) return String.Empty;

            var output = $"Number of Private Recipes : {privateRecipes.Count}\n\n";

            int count = 0;
            foreach (var privateRecipe in privateRecipes)
            {
                count++;
                output += $"Recipe {count}\n{privateRecipe}\n";
            }

            return output;
        }

    }
}

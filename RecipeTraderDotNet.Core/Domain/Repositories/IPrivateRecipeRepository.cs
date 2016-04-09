using System.Collections.Generic;
using RecipeTraderDotNet.Core.Domain.Recipe;

namespace RecipeTraderDotNet.Core.Domain.Repositories
{
    public interface IPrivateRecipeRepository : IRepository<PrivateRecipe>
    {
        List<PrivateRecipe> GetUserRecipes(string userId);
    }
}

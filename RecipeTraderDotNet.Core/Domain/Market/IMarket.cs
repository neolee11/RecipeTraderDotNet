using System.Collections.Generic;
using RecipeTraderDotNet.Core.Domain.Recipe;

namespace RecipeTraderDotNet.Core.Domain.Market
{
    public interface IMarket
    {
        List<PublicRecipe> GetAllRecipes();
        string Publish(PrivateRecipe privateRecipe, decimal price);
        PrivateRecipe Purchase(int publicRecipeId, string requestUserId);
        string Review(int publicRecipeId, string reviewerUserId, int rating, string comment);
        string TakeDown(int publicRecipeId, string requestUserId);
        SystemInfo GetSystemInfo();
        string CreateUserMoneyAccount(string userId, decimal initBalance = 100);

    }
}
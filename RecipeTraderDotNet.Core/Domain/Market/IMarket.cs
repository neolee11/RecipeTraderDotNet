using System.Collections.Generic;
using RecipeTraderDotNet.Core.Domain.Recipe;

namespace RecipeTraderDotNet.Core.Domain.Market
{
    public interface IMarket
    {
        List<PublicRecipe> GetAllRecipes();
        bool Publish(PrivateRecipe privateRecipe, decimal price);
        bool TakeDown(int publicRecipeId, string requestUserId);
        void Review(int publicRecipeId, UserReview review);
        PrivateRecipe Purchase(int publicRecipeId);
    }
}
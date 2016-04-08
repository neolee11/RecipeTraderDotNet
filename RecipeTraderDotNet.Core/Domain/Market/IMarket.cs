using RecipeTraderDotNet.Core.Domain.Recipe;

namespace RecipeTraderDotNet.Core.Domain.Market
{
    public interface IMarket
    {
        bool Publish(IPrivateRecipe privateRecipe);
        bool TakeDown(int publicRecipeId);
        void Review(int publicRecipeId, UserReview review);
        PrivateRecipe Purchase(int publicRecipeId);
    }
}
using RecipeTraderDotNet.Core.Domain.Recipe;

namespace RecipeTraderDotNet.Core.Domain.Repositories
{
    public interface IPublicRecipeRepository : IRepository<PublicRecipe>
    {
        PublicRecipe GetByUserIdAndTitle(string userId, string title);
    }
}

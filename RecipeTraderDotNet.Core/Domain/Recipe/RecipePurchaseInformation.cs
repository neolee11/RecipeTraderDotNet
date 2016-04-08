using System;

namespace RecipeTraderDotNet.Core.Domain.Recipe
{
    public class RecipePurchaseInformation
    {
        public PublicRecipe OriginalMarketRecipe { get; set; }
        public DateTime TimePurchased { get; set; }

    }
}
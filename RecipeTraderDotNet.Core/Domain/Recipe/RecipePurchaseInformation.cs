using System;
using RecipeTraderDotNet.Core.Infrastructure;

namespace RecipeTraderDotNet.Core.Domain.Recipe
{
    [Serializable]
    public class RecipePurchaseInformation : BaseEntity
    {
        //todo: Try remove BaseEntity in EF and see the effect
        public PrivateRecipe PrivateRecipe { get; set; } //navigation property
        public PublicRecipe OriginalMarketRecipe { get; set; }
        public DateTime TimePurchased { get; set; }

        public override string ToString()
        {
            return $"Recipe purchased on {TimePurchased.ToLocalTime()}";
        }
    }
}
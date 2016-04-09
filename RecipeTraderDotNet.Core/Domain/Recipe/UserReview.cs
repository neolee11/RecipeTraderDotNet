using System;
using RecipeTraderDotNet.Core.Infrastructure;

namespace RecipeTraderDotNet.Core.Domain.Recipe
{
    [Serializable]
    public class UserReview : BaseEntity
    {
        public PublicRecipe PublicRecipe { get; set; } //navigation property
        public string ReviewerUserId { get; set; }
        public double Rating { get; set; }
        public string Comment { get; set; }
    }
}
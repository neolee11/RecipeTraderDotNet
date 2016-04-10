using System;
using RecipeTraderDotNet.Core.Infrastructure;

namespace RecipeTraderDotNet.Core.Domain.Recipe
{
    [Serializable]
    public class UserReview : BaseEntity
    {
        private double _rating;
        public PublicRecipe PublicRecipe { get; set; } //navigation property
        public string ReviewerUserId { get; set; }

        public double Rating
        {
            get { return _rating; }
            set
            {
                if(value > 5) _rating = 5;
                else if (value < 1) _rating = 1;
                else _rating = value;
            }
        }

        public string Comment { get; set; }
    }
}
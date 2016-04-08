using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeTraderDotNet.Core.Domain.Market;

namespace RecipeTraderDotNet.Core.Domain.Recipe
{
    public class PublicRecipe : RecipeBase, IPublicRecipe
    {
        public double OverallRating
        {
            get
            {
                if (Reviews == null || Reviews.Count == 0) return 0;
                return Reviews.Sum(r => r.Rating) / Reviews.Count;
            }
        } 

        public List<UserReview> Reviews { get; set; }
        public decimal Price { get; set; }
    }
}

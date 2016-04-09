using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeTraderDotNet.Core.Domain.Market;

namespace RecipeTraderDotNet.Core.Domain.Recipe
{
    [Serializable]
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

        public PublicRecipe()
        {
        }

        public PublicRecipe(PrivateRecipe privateRecipe, decimal price)
        {
            if(privateRecipe == null) return;

            this.Title = privateRecipe.Title;
            this.Author = privateRecipe.Author;
            this.TimeCreated = privateRecipe.TimeCreated;
            this.TimeLastModified = privateRecipe.TimeLastModified;
            this.Items = privateRecipe.Items.ConvertAll(item => item.DeepCopy(false, this));
            this.Price = price > 0 ? price : 1; //set to $1 by default
        }

    }
}

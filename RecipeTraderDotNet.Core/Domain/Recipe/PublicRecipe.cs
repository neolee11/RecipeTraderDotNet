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
        public DateTime TimePublished { get; set; }
       
        public static PublicRecipe ConvertFromPrivateRecipe(PrivateRecipe privateRecipe)
        {
            if (privateRecipe == null) return null;

            var pubRecipe = new PublicRecipe();
            pubRecipe.Title = privateRecipe.Title;
            pubRecipe.Author = privateRecipe.Author;
            pubRecipe.TimeCreated = privateRecipe.TimeCreated;
            pubRecipe.TimeLastModified = privateRecipe.TimeLastModified;
            pubRecipe.Items = privateRecipe.Items.ConvertAll(item => item.DeepCopy(false, pubRecipe));

            return pubRecipe;
        }

        public bool AddReview(UserReview review)
        {
            if (this.Reviews == null) this.Reviews = new List<UserReview>();
            this.Reviews.Add(review);
            return true;
        }

    }
}

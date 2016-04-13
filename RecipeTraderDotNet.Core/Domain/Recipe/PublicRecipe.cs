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

        public override string ToString()
        {
            var output = $"{Title} by [{Author}]\nPice: {Price}  Created on: {TimeCreated.ToLocalTime()}  Last Modified on: {TimeLastModified.ToLocalTime()}\n";

            var itemsOutput = string.Empty;
            for (int i = 0; i < Items.Count; i++)
            {
                itemsOutput += $"{i + 1}. {Items[i].ToString()}\n";
            }
            output += itemsOutput;

            output += $"Overall Rating: {OverallRating}\n";
            var reviewsOutput = string.Empty;
            for (int i = 0; i < Reviews.Count; i++)
            {
                reviewsOutput += $"Review {i+1} : {Reviews[i]}\n";
            }

            output += reviewsOutput;
            return output;
        }
    }
}

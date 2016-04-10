using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeTraderDotNet.Core.Domain.Recipe
{
    [Serializable]
    public class PrivateRecipe : RecipeBase, IPrivateRecipe
    {
        public RecipePurchaseInformation PurchaseInformation { get; set; } = null;
        public bool IsPurchased => PurchaseInformation != null;
        public string RecipeStatus
        {
            get
            {
                if (!this.Items.Any())
                {
                    return "Recipe is empty";
                }
                else if (this.Items.TrueForAll(i => i.Status == RecipeItemStatus.Done))
                {
                    return "Done";
                }
                else if (this.Items.TrueForAll(i => i.Status == RecipeItemStatus.New))
                {
                    return "Yet to start";
                }
                else
                {
                    int total = this.Items.Count;
                    int finished = this.Items.Count(i => i.Status == RecipeItemStatus.Done);
                    return $"In progress - {finished} out of {total} or {(double)finished / total:p2} finished";
                }
            }
        }

        public PrivateRecipe(string authorName, string title)
        {
            this.Author = authorName;
            this.Title = title;
        }

        public static PrivateRecipe ConvertFromPublicRecipe(PublicRecipe publicRecipe)
        {
            var privateRecipe = new PrivateRecipe(publicRecipe.Author, publicRecipe.Title);
            privateRecipe.Items = publicRecipe.Items.ConvertAll(item => item.DeepCopy(false, privateRecipe));
            privateRecipe.TimeCreated = publicRecipe.TimeCreated;
            privateRecipe.TimeLastModified = publicRecipe.TimeLastModified;

          

            return privateRecipe;
        }

        public void Add(RecipeItem item)
        {
            this.Items.Add(item);
            this.TimeLastModified = DateTime.UtcNow;
        }

        public void Insert(RecipeItem item, int index)
        {
            this.Items.Insert(index, item);
            this.TimeLastModified = DateTime.UtcNow;
        }

        public void Remove(RecipeItem item)
        {
            this.Items.Remove(item);
            this.TimeLastModified = DateTime.UtcNow;
        }

        public void Clear()
        {
            this.Items.Clear();
            this.TimeLastModified = DateTime.UtcNow;
        }
    }


}

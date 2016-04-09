using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeTraderDotNet.Core.Infrastructure;

namespace RecipeTraderDotNet.Core.Domain.Recipe
{
    [Serializable]
    public abstract class RecipeBase : BaseEntity
    {
        public string Title { get; set; } //For public recipe, name should be CreatorAccount/RecipeName
        public string Author { get; set; }
        public List<RecipeItem> Items { get; set; } = new List<RecipeItem>(); //take virtual out since can't do lazy loading in repository functions
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
        public virtual DateTime TimeLastModified { get;  set; } = DateTime.UtcNow; //last time a recipe is changed, added/remove/edit item, etc NOT an item's status change. Make virtual so Moq can test
    }

}

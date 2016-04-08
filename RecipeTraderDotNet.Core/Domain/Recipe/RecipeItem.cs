using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeTraderDotNet.Core.Infrastructure;

namespace RecipeTraderDotNet.Core.Domain.Recipe
{
    public class RecipeItem : BaseEntity
    {
        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                ParentRecipe.TimeLastModified = DateTime.UtcNow;
            }
        }

        public RecipeBase ParentRecipe { get; }

        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
        public RecipeItemStatus Status { get; set; } = RecipeItemStatus.New;
        public DateTime TimeLastStatusChange { get; set; } = DateTime.UtcNow;
        

        public RecipeItem(string description, RecipeBase parentRecipe)
        {
            _description = description;
            ParentRecipe = parentRecipe;
        }

        public void Finish()
        {
            this.Status = RecipeItemStatus.Done;
            this.TimeLastStatusChange = DateTime.UtcNow;
        }

        public void Reset()
        {
            this.Status = RecipeItemStatus.New;
            this.TimeLastStatusChange = DateTime.UtcNow;
        }
    }
}

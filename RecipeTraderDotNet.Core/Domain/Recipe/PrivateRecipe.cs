using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeTraderDotNet.Core.Domain.Recipe
{
    public class PrivateRecipe : RecipeBase, IPrivateRecipe
    {
        public DateTime? TimePurchased { get; set; }
        public bool IsPurchased { get; set; }
       
        //Execution
        public RecipeExecutionStatus RecipeExecutionStatus { get; set; }
        public DateTime TimeLastStatusChange { get; set; }

        public PrivateRecipe()
        {
            var now = DateTime.Now;
            TimeCreated = now;
            TimeLastModified = now;
            TimeLastStatusChange = now;
            RecipeExecutionStatus = RecipeExecutionStatus.New;

            TimePurchased = null;
            IsPurchased = false;
        }

        public void Add(RecipeItem item)
        {
            this.Items.Add(item);
        }

        public void InsertAt(RecipeItem item, int index)
        {
            this.Items.Insert(index, item);
        }

        public void Remove(RecipeItem item)
        {
            this.Items.Remove(item);
        }

        public void Clear()
        {
            this.Items.Clear();
        }
    }
}

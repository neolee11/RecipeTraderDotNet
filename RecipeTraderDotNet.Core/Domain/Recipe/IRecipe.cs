using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeTraderDotNet.Core.Domain.Recipe
{
    public interface IRecipe
    {

        void Add(RecipeItem item);
        void Remove(RecipeItem item);
        void InsertAt(RecipeItem item, int index);
        void Clear();
    }

    public abstract class RecipeBase
    {
        public int Id { get; set; }

        public RecipeVisibility RecipeVisibility { get; set; }
               
        public RecipeStatus RecipeStatus { get; set; }
        public int ItemCount { get; set; }

        public string Creator { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeLastModified { get; set; }
        public DateTime TimeLastStatusChange { get; set; }
        public DateTime TimePurchased { get; set; }
    }
}

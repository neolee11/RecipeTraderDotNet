using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeTraderDotNet.Core.Infrastructure;

namespace RecipeTraderDotNet.Core.Domain.Recipe
{
    public class RecipeBase : BaseEntity
    {
        public string Name { get; set; }

        //public bool IsAuthor => true; //check equality of the user name and creator
        public List<RecipeItem> Items { get; set; } = new List<RecipeItem>(); //take virtual out since can't do lazy loading in repository functions

        public string Author { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeLastModified { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeTraderDotNet.Core.Domain.Recipe
{
    public class PublicRecipe : RecipeBase, IPublicRecipe
    {
        public double Rating { get; set; }
        public decimal Price { get; set; }
    }
}

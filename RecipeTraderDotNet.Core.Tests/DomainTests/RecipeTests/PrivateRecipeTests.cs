using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeTraderDotNet.Core.Domain.Recipe;
using Should;
using Xunit;

namespace RecipeTraderDotNet.Core.Tests.DomainTests.RecipeTests
{
    public class PrivateRecipeTests
    {
        [Fact]
        public void ShouldAbleToAddNewItem()
        {
            var recipe = new PrivateRecipe();
            recipe.Add(new RecipeItem());
            recipe.Items.Count.ShouldEqual(1);
        }

        [Fact]
        public void ShouldAbleToSaveRecipe()
        {
            var recipe = new PrivateRecipe();
            var item = new RecipeItem();
            item.Description = "Do something";
            recipe.Add(item);

        }
    }
}

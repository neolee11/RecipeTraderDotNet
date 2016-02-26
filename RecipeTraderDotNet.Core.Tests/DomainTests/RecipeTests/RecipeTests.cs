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
    public class RecipeTests
    {
        [Fact]
        public void BaseTest()
        {
            var a = 1;
            //Assert.Equal(a, 1);
            a.ShouldEqual(1);
        }

        [Fact]
        public void ShouldAbleToCreatePublicRecipe()
        {
            //var user = 

            var recipe = new PublicRecipe();
            //recipe.ShouldBeType(typeof (RecipeBase));
            recipe.ShouldImplement(typeof (RecipeBase));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeTraderDotNet.Core.Domain.Market;
using Should;
using Xunit;

namespace RecipeTraderDotNet.Core.Tests.DomainTests.MarketTests
{
    public class MarketTests
    {
        [Fact]
        public void ConstructorShouldLoadAllPublicRecipes()
        {
            var sut = new Market();
            sut.PublicRecipes.ShouldNotBeNull();
        }

        [Fact]
        public void PublishShouldAddPrivateRecipeToPublicRecipeList()
        {
            
        }
    }
}

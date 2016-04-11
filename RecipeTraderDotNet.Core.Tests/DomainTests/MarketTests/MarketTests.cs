using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using RecipeTraderDotNet.Core.Domain.Market;
using RecipeTraderDotNet.Core.Domain.Repositories;
using Should;
using Xunit;

namespace RecipeTraderDotNet.Core.Tests.DomainTests.MarketTests
{
    public class MarketTests
    {
        [Fact]
        public void PublishShouldAddPrivateRecipeToPublicRecipeList() 
        {
            var mockPublicRecipeRepo = new Mock<IPublicRecipeRepository>();
            var mockMoneyAccoutRepo = new Mock<IMoneyAccountRepository>();

            var sut = new Market(mockPublicRecipeRepo.Object, mockMoneyAccoutRepo.Object);
        }
    }
}

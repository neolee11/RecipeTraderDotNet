using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using RecipeTraderDotNet.Core.Domain.Market;
using RecipeTraderDotNet.Core.Domain.Recipe;
using RecipeTraderDotNet.Core.Domain.Repositories;
using RecipeTraderDotNet.Core.Domain.User;
using RecipeTraderDotNet.TestObjectGenerator;
using Should;
using Xunit;

namespace RecipeTraderDotNet.Core.Tests.DomainTests.UserTests
{
    public class UserContextTests
    {
        private string _userId = "testUser1";
        private Mock<IMarket> _mockMarket = new Mock<IMarket>();
        private Mock<IPrivateRecipeRepository> _mockPrivateRecipeRepo = new Mock<IPrivateRecipeRepository>();
        private Mock<IPublicRecipeRepository> _mockPublicRecipeRepo = new Mock<IPublicRecipeRepository>();
        private Mock<IMoneyAccountRepository> _mockMoneyAccoutRepo = new Mock<IMoneyAccountRepository>();
        private UserContext _sut;

        public UserContextTests()
        {
            _sut = new UserContext(_userId, _mockMarket.Object, _mockPrivateRecipeRepo.Object, _mockPublicRecipeRepo.Object, _mockMoneyAccoutRepo.Object);
        }

        [Fact]
        public void GetUserRecipesShouldGetTheUsersAllRecipes()
        {
            _sut.GetUserRecipes();
            _mockPrivateRecipeRepo.Verify(x => x.GetUserRecipes(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void GetUserMoneyAccountShouldGetTheUsersAccount()
        {
            _sut.GetUserMoneyAccount();
            _mockMoneyAccoutRepo.Verify(x => x.GetUserMoneyAccount(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void PublishRecipeShouldPublishARecipeToMarket()
        {
            _sut.PublishRecipe(234, 5);
            _mockPrivateRecipeRepo.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _mockMarket.Verify(x => x.Publish(It.IsAny<PrivateRecipe>(), It.IsAny<decimal>()), Times.Once);
        }

        [Fact]
        public void ReviewRecipeShouldSendUserReviewToTheMarketRecipe()
        {
            _sut.ReviewRecipe(234, 3, "average");
            _mockMarket.Verify(x => x.Review(It.Is<int>(y => y == 234), It.Is<string>(z => z == _userId), 
                It.Is<int>(b => b == 3), It.Is<string>(a => a == "average")), Times.Once);
        }

        [Fact]
        public void PurchaseRecipeShouldGetAPrivateRecipe()
        {
            var stubPublicRecipe = TestObjectsGenerator.GenerateRandomPublicRecipe(2);
            var stubMyPrivateRecipes = new List<PrivateRecipe>
            {
                TestObjectsGenerator.GenerateRandomPrivateRecipe(2),
                TestObjectsGenerator.GenerateRandomPrivateRecipe(3),
                TestObjectsGenerator.GenerateRandomPrivateRecipe(4),
            };

            _mockPublicRecipeRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns(() => stubPublicRecipe);
            _mockPrivateRecipeRepo.Setup(x => x.GetUserRecipes(It.IsAny<string>())).Returns(() => stubMyPrivateRecipes);
            _mockMarket.Setup(x => x.Purchase(It.IsAny<int>(), It.IsAny<string>())).Returns(() => stubMyPrivateRecipes[1]);
            var result = _sut.PurchaseRecipe(1223);
            result.ShouldEqual(string.Empty);

            _mockPublicRecipeRepo.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _mockPrivateRecipeRepo.Verify(x => x.GetUserRecipes(It.IsAny<string>()), Times.Once);
            _mockMarket.Verify(x => x.Purchase(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            _mockPrivateRecipeRepo.Verify(x => x.Insert(It.IsAny<PrivateRecipe>()), Times.Once);
        }
      
        [Fact]
        public void TakeDownRecipeShouldRemoveRecipeFromMarket()
        {
            _sut.TakeDownRecipe(123);
            _mockMarket.Verify(x => x.TakeDown(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }
    }
}

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
using RecipeTraderDotNet.Core.Tests.Utilities;
using RecipeTraderDotNet.TestObjectGenerator;
using Should;
using Xunit;

namespace RecipeTraderDotNet.Core.Tests.DomainTests.MarketTests
{
    public class MarketTests
    {
        private Mock<IPublicRecipeRepository> _mockPublicRecipeRepo = new Mock<IPublicRecipeRepository>();
        private Mock<IMoneyAccountRepository> _mockMoneyAccoutRepo = new Mock<IMoneyAccountRepository>();
        private Market _sut;

        public MarketTests()
        {
            _sut = new Market(_mockPublicRecipeRepo.Object, _mockMoneyAccoutRepo.Object);
        }

        [Fact]
        public void GetAllRecipesShoudGetAllPublicRecipes()
        {
            _mockPublicRecipeRepo.Setup(x => x.GetAll());
            _sut.GetAllRecipes();
            _mockPublicRecipeRepo.Verify();
        }

        [Fact]
        public void PublishShouldAddPrivateRecipeToPublicRecipeList() 
        {
            var privateRecipe = TestObjectsGenerator.GenerateRandomPrivateRecipe(4);
            var price = 5;
            _mockPublicRecipeRepo.Setup(x => x.GetByUserIdAndTitle(It.IsAny<string>(), It.IsAny<string>())).Returns(() => null);

            var result = _sut.Publish(privateRecipe, price);

            _mockPublicRecipeRepo.Verify(x => x.Insert(It.IsAny<PublicRecipe>()), Times.Exactly(1));
            result.ShouldEqual(string.Empty);
        }

        [Fact]
        public void PublishShouldDisallowPublishingDuplicate()
        {
            var privateRecipe = TestObjectsGenerator.GenerateRandomPrivateRecipe(4);
            var price = 5;
            _mockPublicRecipeRepo.Setup(x => x.GetByUserIdAndTitle(It.IsAny<string>(), It.IsAny<string>())).Returns(() => new PublicRecipe());

            var result = _sut.Publish(privateRecipe, price);

            _mockPublicRecipeRepo.Verify(x => x.Insert(It.IsAny<PublicRecipe>()), Times.Never);
            result.ShouldNotEqual(string.Empty);
        }

        [Fact]
        public void PurchaseShouldRetrieveAnPrivateRecipe()
        {
            var stubPublicRecipe = TestObjectsGenerator.GenerateRandomPublicRecipe(2);
            var stubBuyerAccount = TestObjectsGenerator.GenerateRandoMoneyAccount();
            stubBuyerAccount.Balance += stubPublicRecipe.Price + 10;
            var stubSellerAccount = TestObjectsGenerator.GenerateRandoMoneyAccount();
            
            _mockPublicRecipeRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns(() => stubPublicRecipe);
            _mockMoneyAccoutRepo.Setup(x => x.GetUserMoneyAccount(It.Is<String>(y => y == stubBuyerAccount.UserId))).Returns(() => stubBuyerAccount);
            _mockMoneyAccoutRepo.Setup(x => x.GetUserMoneyAccount(It.Is<String>(y => y == stubPublicRecipe.Author))).Returns(() => stubSellerAccount);

            var result = _sut.Purchase(stubPublicRecipe.Id, stubBuyerAccount.UserId);

            _mockPublicRecipeRepo.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _mockMoneyAccoutRepo.Verify(x => x.Update(It.IsAny<MoneyAccount>()), Times.Exactly(2));
            result.ShouldNotBeNull();
            result.Author.ShouldEqual(stubPublicRecipe.Author);
            result.Items.Count.ShouldEqual(stubPublicRecipe.Items.Count);
            result.PurchaseInformation.ShouldNotBeNull();
            result.PurchaseInformation.PrivateRecipe.ShouldBeSameAs(result);
            result.PurchaseInformation.OriginalMarketRecipe.ShouldBeSameAs(stubPublicRecipe);
            result.PurchaseInformation.TimePurchased.ShouldBeInRange(DateTime.UtcNow.CreateTimeConstraintLowerBound(), DateTime.UtcNow.CreateTimeConstraintUpperBound());
        }

        [Fact]
        public void PurchaseShouldApplyCorrectMoneyTransactionWhenBuyerIsNotRecipeAuthor()
        {
            var stubPublicRecipe = TestObjectsGenerator.GenerateRandomPublicRecipe(2);
            var stubBuyerAccount = TestObjectsGenerator.GenerateRandoMoneyAccount();
            stubBuyerAccount.Balance += stubPublicRecipe.Price + 10;
            var stubSellerAccount = TestObjectsGenerator.GenerateRandoMoneyAccount();

            var buyerOrigBalance = stubBuyerAccount.Balance;
            var sellerOrigBalance = stubSellerAccount.Balance;

            _mockPublicRecipeRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns(() => stubPublicRecipe);
            _mockMoneyAccoutRepo.Setup(x => x.GetUserMoneyAccount(It.Is<String>(y => y == stubBuyerAccount.UserId))).Returns(() => stubBuyerAccount);
            _mockMoneyAccoutRepo.Setup(x => x.GetUserMoneyAccount(It.Is<String>(y => y == stubPublicRecipe.Author))).Returns(() => stubSellerAccount);

            var result = _sut.Purchase(stubPublicRecipe.Id, stubBuyerAccount.UserId);
            stubBuyerAccount.Balance.ShouldEqual(buyerOrigBalance - stubPublicRecipe.Price);
            stubSellerAccount.Balance.ShouldEqual(sellerOrigBalance + stubPublicRecipe.Price);
        }

        [Fact]
        public void PurchaseShouldNotApplyMoneyTransactionWhenBuyerIsRecipeAuthor()
        {
            var stubPublicRecipe = TestObjectsGenerator.GenerateRandomPublicRecipe(2);
            var stubBuyerAccount = TestObjectsGenerator.GenerateRandoMoneyAccount();
            stubBuyerAccount.Balance = stubPublicRecipe.Price + 10;
            stubBuyerAccount.UserId = stubPublicRecipe.Author;
            var stubSellerAccount = TestObjectsGenerator.GenerateRandoMoneyAccount();

            var buyerOrigBalance = stubBuyerAccount.Balance;
            var sellerOrigBalance = stubSellerAccount.Balance;

            _mockPublicRecipeRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns(() => stubPublicRecipe);
            _mockMoneyAccoutRepo.Setup(x => x.GetUserMoneyAccount(It.Is<String>(y => y == stubBuyerAccount.UserId))).Returns(() => stubBuyerAccount);
            _mockMoneyAccoutRepo.Setup(x => x.GetUserMoneyAccount(It.Is<String>(y => y == stubPublicRecipe.Author))).Returns(() => stubSellerAccount);

            var result = _sut.Purchase(stubPublicRecipe.Id, stubBuyerAccount.UserId);
            stubBuyerAccount.Balance.ShouldEqual(buyerOrigBalance);
            stubSellerAccount.Balance.ShouldEqual(sellerOrigBalance);
        }

        [Fact]
        public void PurchaseShouldDisallowTransactionIfBuyerHasInsufficientFund()
        {
            var stubPublicRecipe = TestObjectsGenerator.GenerateRandomPublicRecipe(2);
            var stubBuyerAccount = TestObjectsGenerator.GenerateRandoMoneyAccount();
            stubBuyerAccount.Balance = stubPublicRecipe.Price - 1;
            var stubSellerAccount = TestObjectsGenerator.GenerateRandoMoneyAccount();

            var buyerOrigBalance = stubBuyerAccount.Balance;
            var sellerOrigBalance = stubSellerAccount.Balance;

            _mockPublicRecipeRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns(() => stubPublicRecipe);
            _mockMoneyAccoutRepo.Setup(x => x.GetUserMoneyAccount(It.Is<String>(y => y == stubBuyerAccount.UserId))).Returns(() => stubBuyerAccount);
            _mockMoneyAccoutRepo.Setup(x => x.GetUserMoneyAccount(It.Is<String>(y => y == stubPublicRecipe.Author))).Returns(() => stubSellerAccount);

            var result = _sut.Purchase(stubPublicRecipe.Id, stubBuyerAccount.UserId);
            result.ShouldBeNull();
            stubBuyerAccount.Balance.ShouldEqual(buyerOrigBalance);
            stubSellerAccount.Balance.ShouldEqual(sellerOrigBalance);
        }

        [Fact]
        public void ReviewShouldAddReviewToThePublicRecipe()
        {
            var stubPublicRecipe = TestObjectsGenerator.GenerateRandomPublicRecipe(2);
            var rating = 4;
            var comment = "good recipe";

            _mockPublicRecipeRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns(() => stubPublicRecipe);

            var result = _sut.Review(stubPublicRecipe.Id, "some user", rating, comment);

            _mockPublicRecipeRepo.Verify(x => x.Update(It.IsAny<PublicRecipe>()), Times.Once);
            result.ShouldEqual(string.Empty);
            stubPublicRecipe.Reviews[stubPublicRecipe.Reviews.Count - 1].Rating.ShouldEqual(rating);
            stubPublicRecipe.Reviews[stubPublicRecipe.Reviews.Count - 1].Comment.ShouldEqual(comment);
        }

        [Fact]
        public void TakeDownShouldRemoveOwnPublicRecipeFromMarket()
        {
            var stubPublicRecipe = TestObjectsGenerator.GenerateRandomPublicRecipe(2);
            _mockPublicRecipeRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns(() => stubPublicRecipe);

            var result = _sut.TakeDown(stubPublicRecipe.Id, stubPublicRecipe.Author);

            result.ShouldEqual(string.Empty);
            _mockPublicRecipeRepo.Verify(x => x.Delete(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void TakeDownShouldPreventTakingDownOtherPeoplesRecipe()
        {
            var stubPublicRecipe = TestObjectsGenerator.GenerateRandomPublicRecipe(2);
            _mockPublicRecipeRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns(() => stubPublicRecipe);

            var result = _sut.TakeDown(stubPublicRecipe.Id, stubPublicRecipe.Author + "afd");

            result.ShouldNotEqual(string.Empty);
            _mockPublicRecipeRepo.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void GetSystemInfoShouldRetrieveCurrentSystemState()
        {
            var stubAccounts = new List<MoneyAccount>
            {
                TestObjectsGenerator.GenerateRandoMoneyAccount(),
                TestObjectsGenerator.GenerateRandoMoneyAccount(),
                TestObjectsGenerator.GenerateRandoMoneyAccount()
            };

            decimal totalCurrency = 0;
            foreach (var stubAccount in stubAccounts)
            {
                totalCurrency += stubAccount.Balance;
            }

            _mockMoneyAccoutRepo.Setup(x => x.GetAll()).Returns(() => stubAccounts);

            var result = _sut.GetSystemInfo();
            _mockMoneyAccoutRepo.Verify(x => x.GetAll(), Times.Once);
            result.TotalUsers.ShouldEqual(stubAccounts.Count);
            result.TotalCurrency.ShouldEqual(totalCurrency);
        }

    }
}

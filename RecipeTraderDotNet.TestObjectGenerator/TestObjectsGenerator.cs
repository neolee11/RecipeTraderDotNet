using System;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Kernel;
using RecipeTraderDotNet.Core.Domain.Recipe;
using RecipeTraderDotNet.Core.Domain.User;

namespace RecipeTraderDotNet.TestObjectGenerator
{
    public static class TestObjectsGenerator
    {
        private static Random random = new Random();

        public static PrivateRecipe GenerateRandomPrivateRecipe(int numOfItems = 3)
        {
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var privateRecipe = fixture.Build<PrivateRecipe>().Without(r => r.Items).Create();

            for (int i = 0; i < numOfItems; i++)
            {
                var item = GenerateRandomRecipeItem(privateRecipe, true);
                privateRecipe.Items.Add(item);
            }

            return privateRecipe;
        }

        public static PublicRecipe GenerateRandomPublicRecipe(int numOfItems = 3)
        {
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var publicRecipe = fixture.Build<PublicRecipe>().Without(r => r.Items).Create();
            for (int i = 0; i < numOfItems; i++)
            {
                var item = GenerateRandomRecipeItem(publicRecipe, false);
                publicRecipe.Items.Add(item);
            }

            for (int i = 0; i < publicRecipe.Reviews.Count; i++)
            {
                var review = publicRecipe.Reviews[i];
                review.PublicRecipe = publicRecipe;
                review.Rating = GenerateRandomReviewScore();
            }

            return publicRecipe;
        }

        public static RecipeItem GenerateRandomRecipeItem(RecipeBase parentRecipe, bool usePrivateRecipe)
        {
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            fixture.Customizations.Add(usePrivateRecipe
                ? new TypeRelay(typeof(RecipeBase), typeof(PrivateRecipe))
                : new TypeRelay(typeof(RecipeBase), typeof(PublicRecipe)));

            var item = fixture.Build<RecipeItem>().Create();
            item.ParentRecipe = parentRecipe;
            return item;
        }

        public static RecipeItem GenerateRandomRecipeItemWithAutoMoq()
        {
            var fixture = new Fixture();
            //fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            //fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            fixture.Customize(new AutoMoqCustomization());

            var mockGateway = fixture.Freeze<Mock<RecipeBase>>(); //get the mocked object so we can test against it later

            var item = fixture.Create<RecipeItem>();
            return item;
        }

        public static UserReview GenerateRandomReview(PublicRecipe publicRecipe)
        {
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var review = fixture.Build<UserReview>().With(r => r.PublicRecipe, publicRecipe).Create();
            return review;
        }

        private static int GenerateRandomReviewScore()
        {
            return random.Next(1, 6);
        }

        public static MoneyAccount GenerateRandoMoneyAccount()
        {
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var account = fixture.Create<MoneyAccount>();
            return account;
        }

    }
}

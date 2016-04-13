using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using RecipeTraderDotNet.Core.Domain.Recipe;
using RecipeTraderDotNet.Core.Domain.User;

namespace RecipeTraderDotNet.Core.Application
{
    public static class DomainObjectsGenerator
    {
        private static Random random = new Random();

        public static PrivateRecipe GenerateRandomPrivateRecipe(int numOfItems = 3, string author = "", bool isPurchase = false)
        {
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var privateRecipe = fixture.Build<PrivateRecipe>().Without(r => r.Items).Create();

            privateRecipe.Id = GenerateRandomPositiveInteger();
            for (int i = 0; i < numOfItems; i++)
            {
                var item = GenerateRandomRecipeItem(privateRecipe, true);
                privateRecipe.Items.Add(item);
            }

            if (!string.IsNullOrEmpty(author))
            {
                privateRecipe.Author = author;
                privateRecipe.OwnerUserId = author;
            }

            if (!isPurchase)
            {
                privateRecipe.PurchaseInformation = null;
            }

            return privateRecipe;
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
            item.Id = GenerateRandomPositiveInteger();
            return item;
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

            publicRecipe.Id = GenerateRandomPositiveInteger();

            return publicRecipe;
        }

        public static UserReview GenerateRandomReview(PublicRecipe publicRecipe)
        {
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var review = fixture.Build<UserReview>().With(r => r.PublicRecipe, publicRecipe).Create();
            review.Id = GenerateRandomPositiveInteger();
            return review;
        }

        private static int GenerateRandomReviewScore()
        {
            return random.Next(1, 6);
        }

        public static MoneyAccount GenerateRandoMoneyAccount(string userId = "", int balance = 100)
        {
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var account = fixture.Create<MoneyAccount>();

            account.Id = GenerateRandomPositiveInteger();
            if (!string.IsNullOrEmpty(userId))
            {
                account.UserId = userId;
            }

            account.Balance = balance > 0 ? balance : 100;

            return account;
        }

        private static int GenerateRandomPositiveInteger()
        {
            return random.Next(1, Int32.MaxValue);
        }

    }
}

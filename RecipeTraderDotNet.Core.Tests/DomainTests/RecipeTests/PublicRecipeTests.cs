using RecipeTraderDotNet.Core.Domain.Recipe;
using Should;
using Xunit;

namespace RecipeTraderDotNet.Core.Tests.DomainTests.RecipeTests
{
    public class PublicRecipeTests
    {
        private const string _testAuthor = "Test author";
        private const string _testTitle = "Test title";

        [Fact]
        public void ConvertShouldConvertPrivateRecipeToPublicRecipe()
        {
            var privateRecipe = new PrivateRecipe(_testAuthor, _testTitle);
            privateRecipe.Add(new RecipeItem("test1", privateRecipe));
            privateRecipe.Add(new RecipeItem("test2", privateRecipe));
            privateRecipe.Add(new RecipeItem("test3", privateRecipe));

            var sut = PublicRecipe.ConvertFromPrivateRecipe(privateRecipe);

            sut.Id.ShouldEqual(0);
            sut.Author.ShouldEqual(privateRecipe.Author);
            sut.Title.ShouldEqual(privateRecipe.Title);
            sut.TimeCreated.ShouldEqual(privateRecipe.TimeCreated);
            sut.TimeLastModified.ShouldEqual(privateRecipe.TimeLastModified);
            sut.Items.Count.ShouldEqual(privateRecipe.Items.Count);
            sut.Items[1].Description.ShouldEqual(privateRecipe.Items[1].Description);
            sut.Items[1].Description.ShouldNotEqual(privateRecipe.Items[0].Description);
        }

        [Theory]
        [InlineData(new int[] {2, 4}, 3)]
        [InlineData(new int[] {2, 5, 2}, 3)]
        public void OverallRatingShouldComputerCorrectly(int[] ratings, int expected)
        {
            var sut = new PublicRecipe();
            foreach (var rating in ratings)
            {
                sut.AddReview(new UserReview {Rating = rating});
            }
            
            sut.OverallRating.ShouldEqual(expected);
            
        }
    }
}
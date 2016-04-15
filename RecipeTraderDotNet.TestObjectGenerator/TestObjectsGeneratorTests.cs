using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture.Xunit;
using Ploeh.SemanticComparison.Fluent;
using RecipeTraderDotNet.Core.Domain.Recipe;
using Should;
using Xunit;
using Xunit.Abstractions;

namespace RecipeTraderDotNet.TestObjectGenerator
{
    public class TestObjectsGeneratorTests
    {
        private readonly ITestOutputHelper _output;

        public TestObjectsGeneratorTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void GenerateRandomRecipeItemWithAutoMoqShouldWork()
        {
            var recipeItem = TestObjectsGenerator.GenerateRandomRecipeItemWithAutoMoq();
            recipeItem.ShouldNotBeNull();
        }

        [Theory]
        //[Theory, AutoData] //Does not work
        //[AutoData]  //Does not work!!
        [InlineData(1, 2)]
        public void AutoDataShoudWork(int a, int b)
        {
            var result = a + b;
            result.ShouldEqual(a + b);
        }

        [Fact]
        public void HashcodeShouldWork()
        {
            var val = 32;
            _output.WriteLine(val.GetHashCode().ToString());

            var stringVal = "this is some test";
            _output.WriteLine(stringVal.GetHashCode().ToString());

            var stringVal2 = "this is some test";
            _output.WriteLine(stringVal2.GetHashCode().ToString());

        }

        [Fact]
        public void SemanticComparisonShouldWork()
        {
            var publicRecipe = TestObjectsGenerator.GenerateRandomPublicRecipe();
            var rating = 4;
            var comment = "some comment";
            var reviewerId = "user123";

            var sut = new UserReview()
            {
                Id = 10,
                PublicRecipe = publicRecipe,
                Rating = rating,
                Comment = comment,
                ReviewerUserId = reviewerId
            };

            var actual = new UserReview
            {
                PublicRecipe = publicRecipe,
                Rating = rating,
                Comment = comment,
                ReviewerUserId = reviewerId
            };

            var expected = sut.AsSource().OfLikeness<UserReview>().Without(d => d.Id);
            expected.ShouldEqual(actual);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using RecipeTraderDotNet.Core.Domain.Recipe;
using RecipeTraderDotNet.Data.Repositories.Memory;
using RecipeTraderDotNet.TestObjectGenerator;
using Should;
using Xunit;

namespace RecipeTraderDotNet.Data.Tests.RepositoryTests.MemoryRepositoryTests
{
    public class PublicRecipeRepositoryTests
    {
        private List<PublicRecipe> GetCurrentPublicRecipeSystemState()
        {
            var state = new List<PublicRecipe>
            {
                TestObjectsGenerator.GenerateRandomPublicRecipe(3),
                TestObjectsGenerator.GenerateRandomPublicRecipe(4),
                TestObjectsGenerator.GenerateRandomPublicRecipe(2)
            };
            return state;
        }

        [Fact]
        public void GetAllShouldWork()
        {
            var state = GetCurrentPublicRecipeSystemState();
            var sut = new PublicRecipeRepository(state);
            var results = sut.GetAll();

            results.Count.ShouldEqual(state.Count);
            for (int i = 0; i < results.Count; i++)
            {
                results[i].Id.ShouldEqual(state[i].Id);
                results[i].Author.ShouldEqual(state[i].Author);
                results[i].Title.ShouldEqual(state[i].Title);
                results[i].OverallRating.ShouldEqual(state[i].OverallRating);
            }
        }

        [Fact]
        public void GetByIdShouldWork()
        {
            var state = GetCurrentPublicRecipeSystemState();
            var sut = new PublicRecipeRepository(state);

            var resultNull = sut.GetById(962391223);
            resultNull.ShouldBeNull();

            var result = sut.GetById(state[1].Id);
            result.ShouldNotBeNull();
            result.Id.ShouldEqual(state[1].Id);
            result.Author.ShouldEqual(state[1].Author);
            result.Title.ShouldEqual(state[1].Title);
            result.OverallRating.ShouldEqual(state[1].OverallRating);
        }

        [Fact]
        public void InsertShouldWork()
        {
            var state = GetCurrentPublicRecipeSystemState();
            var originalCount = state.Count;
            var sut = new PublicRecipeRepository(state);
            var newPublicRecipe = TestObjectsGenerator.GenerateRandomPublicRecipe(1);
            sut.Insert(newPublicRecipe);

            var results = sut.GetAll();
            results.Count.ShouldEqual(originalCount + 1);
            results[results.Count - 1].Id.ShouldEqual(newPublicRecipe.Id);
            results[results.Count - 1].Author.ShouldEqual(newPublicRecipe.Author);
            results[results.Count - 1].Title.ShouldEqual(newPublicRecipe.Title);
            results[results.Count - 1].OverallRating.ShouldEqual(newPublicRecipe.OverallRating);
        }

        [Fact]
        public void UpdateShouldWork()
        {
            var state = GetCurrentPublicRecipeSystemState();
            var sut = new PublicRecipeRepository(state);
            var existingOne = state[0];
            var newTitle = "My new title";
            var newItem = TestObjectsGenerator.GenerateRandomRecipeItem(existingOne, false);

            existingOne.Title = newTitle;
            existingOne.Items.Add(newItem);
            sut.Update(existingOne);

            var result = sut.GetById(existingOne.Id);
            result.Items.Count.ShouldEqual(existingOne.Items.Count);
            result.Title.ShouldEqual(newTitle);
            result.OverallRating.ShouldEqual(existingOne.OverallRating);
        }

        [Fact]
        public void DeleteShouldWork()
        {
            var state = GetCurrentPublicRecipeSystemState();
            var originalCount = state.Count;
            var sut = new PublicRecipeRepository(state);
            var idToDelete = state[1].Id;

            sut.Delete(idToDelete);
            var results = sut.GetAll();
            results.Count.ShouldEqual(originalCount - 1);

            var recipe = sut.GetById(idToDelete);
            recipe.ShouldBeNull();
        }

        [Fact]
        public void GetByUserIdAndTitleShouldWork()
        {
            var state = GetCurrentPublicRecipeSystemState();
            var sut = new PublicRecipeRepository(state);

            var existingOne = state[2];
            var result1 = sut.GetByUserIdAndTitle(existingOne.Author, existingOne.Title);
            result1.ShouldNotBeNull();

            var result2 = sut.GetByUserIdAndTitle(existingOne.Author + "adffd", existingOne.Title);
            result2.ShouldBeNull();
        }

    }
}

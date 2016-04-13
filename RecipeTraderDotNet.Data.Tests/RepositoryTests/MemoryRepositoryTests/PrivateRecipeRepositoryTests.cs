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
    public class PrivateRecipeRepositoryTests
    {
        private List<PrivateRecipe> GetCurrentPrivateRecipeSystemState()
        {
            var state = new List<PrivateRecipe>();
            state.Add(TestObjectsGenerator.GenerateRandomPrivateRecipe(3));
            state.Add(TestObjectsGenerator.GenerateRandomPrivateRecipe(4));
            state.Add(TestObjectsGenerator.GenerateRandomPrivateRecipe(2));
            return state;
        }

        [Fact]
        public void GetAllShouldWork()
        {
            var state = GetCurrentPrivateRecipeSystemState();
            var sut = new PrivateRecipeRepository(state);
            var results = sut.GetAll();

            results.Count.ShouldEqual(state.Count);
            for (int i = 0; i < results.Count; i++)
            {
                results[i].Id.ShouldEqual(state[i].Id);
                results[i].Author.ShouldEqual(state[i].Author);
                results[i].Title.ShouldEqual(state[i].Title);
            }
        }

        [Fact]
        public void GetByIdShouldWork()
        {
            var state = GetCurrentPrivateRecipeSystemState();
            var sut = new PrivateRecipeRepository(state);

            var resultNull = sut.GetById(962301823);
            resultNull.ShouldBeNull();

            var result = sut.GetById(state[1].Id);
            result.ShouldNotBeNull();
            result.Id.ShouldEqual(state[1].Id);
            result.Author.ShouldEqual(state[1].Author);
            result.Title.ShouldEqual(state[1].Title);
        }

        [Fact]
        public void InsertShouldWork()
        {
            var state = GetCurrentPrivateRecipeSystemState();
            var originalCount = state.Count;
            var sut = new PrivateRecipeRepository(state);
            var newPrivateRecipe = TestObjectsGenerator.GenerateRandomPrivateRecipe(2);
            sut.Insert(newPrivateRecipe);

            var results = sut.GetAll();
            results.Count.ShouldEqual(originalCount + 1);
            results[results.Count - 1].Id.ShouldEqual(newPrivateRecipe.Id);
            results[results.Count - 1].Author.ShouldEqual(newPrivateRecipe.Author);
            results[results.Count - 1].Title.ShouldEqual(newPrivateRecipe.Title);
        }

        [Fact]
        public void UpdateShouldWork()
        {
            var state = GetCurrentPrivateRecipeSystemState();
            var sut = new PrivateRecipeRepository(state);
            var existingOne = state[0];
            var newTitle = "My new title";
            var newItem = TestObjectsGenerator.GenerateRandomRecipeItem(existingOne, true);
            
            existingOne.Title = newTitle;
            existingOne.Add(newItem);
            sut.Update(existingOne);

            var result = sut.GetById(existingOne.Id);
            result.Items.Count.ShouldEqual(existingOne.Items.Count);
            result.Title.ShouldEqual(newTitle);
        }

        [Fact]
        public void DeleteShouldWork()
        {
            var state = GetCurrentPrivateRecipeSystemState();
            var originalCount = state.Count;
            var sut = new PrivateRecipeRepository(state);
            var idToDelete = state[1].Id;

            sut.Delete(idToDelete);
            var results = sut.GetAll();
            results.Count.ShouldEqual(originalCount - 1);

            var recipe = sut.GetById(idToDelete);
            recipe.ShouldBeNull();
        }

        [Fact]
        public void GetUserRecipesShouldWork()
        {
            var state = GetCurrentPrivateRecipeSystemState();
            state[0].OwnerUserId = "user1";
            state[1].OwnerUserId = "user1";
            var sut = new PrivateRecipeRepository(state);

            var results = sut.GetUserRecipes("user1");
            results.Count.ShouldEqual(2);
        }
    }
}

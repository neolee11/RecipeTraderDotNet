using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using RecipeTraderDotNet.Core.Domain.Recipe;
using RecipeTraderDotNet.Core.Tests.Utilities;
using Should;
using Should.Fluent;
using Xunit;

namespace RecipeTraderDotNet.Core.Tests.DomainTests.RecipeTests
{
    public class PrivateRecipeTests
    {
        private readonly string _author;
        private readonly string _title;
        private readonly string _itemDescription;

        public PrivateRecipeTests()
        {
            _author = "daniel";
            _title = "Test recipe";
            _itemDescription = "test description";
        }

        [Fact]
        public void ConstructorShouldCreatePrivateRecipeWithCorrectInformation()
        {
            var recipe = new PrivateRecipe(_author, _title);
            recipe.IsPurchased.ShouldBeFalse();
            recipe.RecipeStatus.Should().Equal("Recipe is empty");
            recipe.TimeCreated.ShouldBeInRange(DateTime.UtcNow.CreateTimeConstraintLowerBound(), DateTime.UtcNow.CreateTimeConstraintUpperBound());
            recipe.TimeLastModified.ShouldEqual(recipe.TimeCreated);
            recipe.Author.ShouldEqual(_author);
            recipe.Title.ShouldEqual(_title);
            recipe.Items.ShouldNotBeNull();
            recipe.Items.ShouldBeEmpty();
        }

        [Fact]
        public void AddShouldAddNewItem()
        {
            var recipe = new PrivateRecipe(_author, _title);
            var mockRecipeItem = new Mock<RecipeItem>(_itemDescription, recipe);

            recipe.Add(mockRecipeItem.Object);

            recipe.Items.Count.ShouldEqual(1);
            recipe.Items[0].ShouldBeSameAs(mockRecipeItem.Object);
            recipe.RecipeStatus.ShouldEqual("Yet to start");
            recipe.TimeLastModified.ShouldBeInRange(DateTime.UtcNow.CreateTimeConstraintLowerBound(), DateTime.UtcNow.CreateTimeConstraintUpperBound());
        }

        [Fact]
        public void AddShouldUpdateTimeLastModified()
        {
            var sut = new PrivateRecipe(_author, _title);
            var mockRecipeItem = new Mock<RecipeItem>(_itemDescription, sut);
            var oldTime = sut.TimeLastModified;

            Thread.Sleep(100);
            sut.Add(mockRecipeItem.Object);
            var newTime = sut.TimeLastModified;

            newTime.ShouldNotEqual(oldTime);
        }

        [Fact]
        public void AddShouldAppendItemToTheEnd()
        {
            var recipe = new PrivateRecipe(_author, _title);
            var mockRecipeItem = new Mock<RecipeItem>(_itemDescription, recipe);
            recipe.Add(mockRecipeItem.Object);
            recipe.Items[recipe.Items.Count - 1].ShouldBeSameAs(mockRecipeItem.Object);
        }

        [Fact]
        public void InsertShouldInsertItemAtSpecifiedIndex()
        {
            var sut = new PrivateRecipe(_author, _title);
            var recipeItem1 = new RecipeItem(_itemDescription, sut);
            var recipeItem2 = new RecipeItem(_itemDescription, sut);

            sut.Add(recipeItem1);
            sut.Insert(recipeItem2, 0);

            sut.Items[0].ShouldBeSameAs(recipeItem2);
        }

        [Fact]
        public void InsertShouldUpdateLastModifiedTime()
        {
            var sut = new PrivateRecipe(_author, _title);
            var recipeItem1 = new RecipeItem(_itemDescription, sut);
            var recipeItem2 = new RecipeItem(_itemDescription, sut);

            sut.Add(recipeItem1);
            var oldTime = sut.TimeLastModified;

            Thread.Sleep(100);

            sut.Insert(recipeItem2, 0);
            var newTime = sut.TimeLastModified;

            newTime.ShouldNotEqual(oldTime);
        }

        [Fact]
        public void RemoveShouldRemoveItem()
        {
            var sut = new PrivateRecipe(_author, _title);
            var recipeItem1 = new RecipeItem(_itemDescription, sut);
            var recipeItem2 = new RecipeItem(_itemDescription, sut);
            sut.Add(recipeItem1);
            sut.Add(recipeItem2);

            sut.Remove(recipeItem1);

            sut.Items.Count.ShouldEqual(1);
            sut.Items[0].ShouldBeSameAs(recipeItem2);
        }

        [Fact]
        public void RemoveShouldUpdateLastModifiedTime()
        {
            var sut = new PrivateRecipe(_author, _title);
            var recipeItem1 = new RecipeItem(_itemDescription, sut);
            var recipeItem2 = new RecipeItem(_itemDescription, sut);
            sut.Add(recipeItem1);
            sut.Add(recipeItem2);

            var oldTime = sut.TimeLastModified;

            Thread.Sleep(100);
            sut.Remove(recipeItem1);
            var newTime = sut.TimeLastModified;

            newTime.ShouldNotEqual(oldTime);
        }

        [Fact]
        public void ClearShouldRemoveAllItems()
        {
            var sut = new PrivateRecipe(_author, _title);
            var recipeItem1 = new RecipeItem(_itemDescription, sut);
            var recipeItem2 = new RecipeItem(_itemDescription, sut);
            sut.Add(recipeItem1);
            sut.Add(recipeItem2);

            sut.Clear();

            sut.Items.Count.ShouldEqual(0);
        }

        [Fact]
        public void ClearShouldUpdateLastModifiedTime()
        {
            var sut = new PrivateRecipe(_author, _title);
            var recipeItem1 = new RecipeItem(_itemDescription, sut);
            var recipeItem2 = new RecipeItem(_itemDescription, sut);
            sut.Add(recipeItem1);
            sut.Add(recipeItem2);
            var oldTime = sut.TimeLastModified;
            Thread.Sleep(100);

            sut.Clear();
            var newTime = sut.TimeLastModified;

            newTime.ShouldNotEqual(oldTime);
        }

    }
}

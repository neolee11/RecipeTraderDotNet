using System;
using System.Threading;
using Moq;
using Ploeh.SemanticComparison.Fluent;
using RecipeTraderDotNet.Core.Domain.Recipe;
using RecipeTraderDotNet.Core.Tests.Utilities;
using Should;
using Xunit;
using Xunit.Abstractions;

namespace RecipeTraderDotNet.Core.Tests.DomainTests.RecipeTests
{
    public class RecipeItemTests
    {
        private readonly Mock<RecipeBase> _mockRecipe;
        private readonly string _description;
        private readonly ITestOutputHelper _output;

        public RecipeItemTests(ITestOutputHelper output)
        {
            _mockRecipe = new Mock<RecipeBase>();
            _description = "Do something";
            _output = output;
        }

        [Fact]
        public void ConstructorShouldInitializeWithCorrectInfo()
        {
            var item = new RecipeItem(_description, _mockRecipe.Object);
            item.Description.ShouldEqual(_description);
            item.ParentRecipe.ShouldBeSameAs(_mockRecipe.Object);
            item.Status.ShouldEqual(RecipeItemStatus.New);
            item.TimeCreated.ShouldBeInRange(DateTime.UtcNow.CreateTimeConstraintLowerBound(), DateTime.UtcNow.CreateTimeConstraintUpperBound());
            item.TimeLastStatusChange.ShouldEqual(item.TimeCreated);
        }

        [Fact]
        public void ModifyDescriptionShouldUpdateLastModifiedTime()
        {
            var item = new RecipeItem(_description, _mockRecipe.Object);
            item.Description = "new description";

            _output.WriteLine($"Object time last modified {_mockRecipe.Object.TimeLastModified}");
            _output.WriteLine($"Current time {DateTime.UtcNow}");

            _mockRecipe.VerifySet(x => x.TimeLastModified = It.IsAny<DateTime>(), Times.Exactly(1));
        }

        [Fact]
        public void FinishShouldSetStatusToDoneAndUpdateTime()
        {
            var sut = new RecipeItem(_description, _mockRecipe.Object);
            var oldTime = sut.TimeLastStatusChange;

            Thread.Sleep(100);
            sut.Finish();

            var newTime = sut.TimeLastStatusChange;
            sut.Status.ShouldEqual(RecipeItemStatus.Done);

            newTime.ShouldNotEqual(oldTime);
        }

        [Fact]
        public void ResetShouldSetStatusToNewAndUpdateTime()
        {
            var sut = new RecipeItem(_description, _mockRecipe.Object);
            var oldTime = sut.TimeLastStatusChange;

            Thread.Sleep(100);
            sut.Reset();

            var newTime = sut.TimeLastStatusChange;
            sut.Status.ShouldEqual(RecipeItemStatus.New);

            newTime.ShouldNotEqual(oldTime);
        }

        [Fact]
        public void DeepCopyShouldMakeANewCopy()
        {
            var realRecipe = new PrivateRecipe("author", "title");
            realRecipe.Id = 100;
            var sut = new RecipeItem(_description, realRecipe);
            sut.Id = 200;

            var newObj = sut.DeepCopy(keepId: true);

            newObj.Id.ShouldEqual(sut.Id);
            newObj.Description.ShouldEqual(sut.Description);
            newObj.ParentRecipe.Id.ShouldEqual(sut.ParentRecipe.Id);

            sut.Description = "Something else";
            newObj.Description.ShouldNotEqual(sut.Description);

            sut.Status = RecipeItemStatus.Done;
            newObj.Status.ShouldNotEqual(sut.Status);
        }

        [Fact]
        public void DeepCopyNotKeepIdShouldSetIdToZero()
        {
            var realRecipe = new PrivateRecipe("author", "title");
            realRecipe.Id = 100;
            var sut = new RecipeItem(_description, realRecipe);
            sut.Id = 200;

            var newObj = sut.DeepCopy(keepId: false);

            newObj.Id.ShouldNotEqual(sut.Id);
            newObj.Id.ShouldEqual(0);
        }

        [Fact]
        public void DeepCopySetNewParentShouldSetNewParent()
        {
            var realRecipe = new PrivateRecipe("author", "title");
            realRecipe.Id = 100;
            var sut = new RecipeItem(_description, realRecipe);
            sut.Id = 200;
            var newRecipe = new PrivateRecipe("author2", "title2");
            newRecipe.Id = realRecipe.Id + 10;

            var newObj = sut.DeepCopy(newParent:newRecipe);

            newObj.ParentRecipe.Id.ShouldNotEqual(sut.ParentRecipe.Id);
            newObj.ParentRecipe.Id.ShouldEqual(newRecipe.Id);
        }

        [Fact]
        public void SystemCopyShouldDoDeepCopy()
        {
            var realRecipe = new PrivateRecipe("author", "title");
            realRecipe.Id = 100;
            var sut = new RecipeItem(_description, realRecipe);
            sut.Id = 200;

            var newObj = sut.Copy();
            
            newObj.Id.ShouldEqual(sut.Id);
            newObj.Description.ShouldEqual(sut.Description);
            newObj.ParentRecipe.Id.ShouldEqual(sut.ParentRecipe.Id);

            sut.Description = "Something else";
            newObj.Description.ShouldNotEqual(sut.Description);

            sut.Status = RecipeItemStatus.Done;
            newObj.Status.ShouldNotEqual(sut.Status);
        }

        [Fact]
        public void DeepCopyUsingSemanticComparisonShouldWork()
        {
            var privateRecipe = TestObjectGenerator.TestObjectsGenerator.GenerateRandomPrivateRecipe(4);
            privateRecipe.Id = 100;
            var sut = new RecipeItem(_description, privateRecipe);
            sut.Id = 200;

            var actual = sut.DeepCopy(false, setStatusNow:false);

            var expected = sut.AsSource().OfLikeness<RecipeItem>().Without(t => t.Id);
            expected.ShouldEqual(actual);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using RecipeTraderDotNet.Core.Domain.Recipe;

namespace RecipeTraderDotNet.Data.Tests.Utilities
{
    public class TestHelper
    {
        public static PrivateRecipe GenerateRandomPrivateRecipe(int numOfItems = 3)
        {
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var privateRecipe = fixture.Build<PrivateRecipe>().Without(r => r.Items).Create();

            for (int i = 0; i < numOfItems; i++)
            {
                var item = GenerateRandomRecipeItem(privateRecipe);
                privateRecipe.Items.Add(item);
            }

            return privateRecipe;
        }

        public static RecipeItem GenerateRandomRecipeItem(RecipeBase parentRecipe)
        {
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            fixture.Customizations.Add(new TypeRelay(typeof(RecipeBase), typeof(PrivateRecipe)));
            var item = fixture.Build<RecipeItem>().Create();
            item.ParentRecipe = parentRecipe;
            return item;
        }
    }
}

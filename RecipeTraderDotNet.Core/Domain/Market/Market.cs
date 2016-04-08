using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using RecipeTraderDotNet.Core.Domain.Recipe;

namespace RecipeTraderDotNet.Core.Domain.Market
{
    public class Market : IMarket
    {
        public bool Publish(IPrivateRecipe privateRecipe)
        {
            throw new NotImplementedException();
        }

        public bool TakeDown(int publicRecipeId)
        {
            throw new NotImplementedException();
        }

        public void Review(int publicRecipeId, UserReview review)
        {
            throw new NotImplementedException();
        }

        public PrivateRecipe Purchase(int publicRecipeId)
        {
            throw new NotImplementedException();
        }
    }
}

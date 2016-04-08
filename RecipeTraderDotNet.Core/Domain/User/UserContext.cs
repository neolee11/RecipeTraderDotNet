using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeTraderDotNet.Core.Domain.Market;
using RecipeTraderDotNet.Core.Domain.Recipe;

namespace RecipeTraderDotNet.Core.Domain.User
{
    public class UserContext
    {
        private IMarket _market;

        public string UserId { get; set; }
        public List<PrivateRecipe> Recipes { get; set; }
        public MoneyAccount MoneyAccount { get; set; }

        public UserContext(IMarket market)
        {
            _market = market;
            Recipes = this.GetUserRecipes();
        }

        private List<PrivateRecipe> GetUserRecipes()
        {
            throw new NotImplementedException();
        }
    }
}

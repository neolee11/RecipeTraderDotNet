using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeTraderDotNet.Core.Domain.Market;
using RecipeTraderDotNet.Core.Domain.Recipe;
using RecipeTraderDotNet.Core.Domain.Repositories;

namespace RecipeTraderDotNet.Core.Domain.User
{
    public class UserContext
    {
        private IMarket _market;
        private readonly IPrivateRecipeRepository _privateRecipeRepo;
        private readonly IMoneyAccountRepository _moneyAccountRepo;

        public string UserId { get; }

        public UserContext(string userId, IMarket market, IPrivateRecipeRepository privateRecipeRepo, IMoneyAccountRepository moneyAccountRepo)
        {
            UserId = userId;
            _market = market;
            _privateRecipeRepo = privateRecipeRepo;
            _moneyAccountRepo = moneyAccountRepo;
        }

        public List<PrivateRecipe> GetUserRecipes()
        {
            return _privateRecipeRepo.GetUserRecipes(UserId);
        }

        public MoneyAccount GetUserMoneyAccount()
        {
            return _moneyAccountRepo.GetUserMoneyAccount(UserId);
        }
    }
}

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
    public class UserService
    {
        private IMarket _market;
        private readonly IPrivateRecipeRepository _privateRecipeRepo;
        private readonly IPublicRecipeRepository _publicRecipeRepo;
        private readonly IMoneyAccountRepository _moneyAccountRepo;

        public string UserId { get; }

        public UserService(string userId, IMarket market, 
            IPrivateRecipeRepository privateRecipeRepo, 
            IPublicRecipeRepository publicRecipeRepo, 
            IMoneyAccountRepository moneyAccountRepo)
        {
            UserId = userId;
            _market = market;
            _privateRecipeRepo = privateRecipeRepo;
            _publicRecipeRepo = publicRecipeRepo;
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

        public string PublishRecipe(int privateRecipeId, decimal price)
        {
            var recipe = _privateRecipeRepo.GetById(privateRecipeId);
            return _market.Publish(recipe, price);
        }

        public string ReviewRecipe(int publicRecipeId, int rating, string comment)
        {
            return _market.Review(publicRecipeId, UserId, rating, comment);
        }

        public string PurchaseRecipe(int publicRecipeId)
        {
            //check if you already purchase this recipe before
            var publicRecipe = _publicRecipeRepo.GetById(publicRecipeId);
            var allMyRecipes = _privateRecipeRepo.GetUserRecipes(UserId);
            if (allMyRecipes.Exists(r => r.Author == publicRecipe.Author && r.Title == publicRecipe.Title))
            {
                return "You already purchased the recipe";
            }

            var privateRecipe = _market.Purchase(publicRecipeId, UserId);
            if (privateRecipe == null)
            {
                return "You don't have enough moenty to buy the recipe";
            }

            _privateRecipeRepo.Insert(privateRecipe);
            return string.Empty;
        }

        public string TakeDownRecipe(int publicRecipeId)
        {
            return _market.TakeDown(publicRecipeId, UserId);
        }

        public string CreateNewPrivateRecipe(string recipeTitle)
        {
            if (_privateRecipeRepo.GetUserRecipes(UserId).Exists(r => String.Equals(r.Title, recipeTitle, StringComparison.CurrentCultureIgnoreCase)))
            {
                return $"{recipeTitle} already exists. Choose another title";
            }

            var recipe = new PrivateRecipe(UserId, recipeTitle);
            _privateRecipeRepo.Insert(recipe);
            return String.Empty;
        }
    }
}

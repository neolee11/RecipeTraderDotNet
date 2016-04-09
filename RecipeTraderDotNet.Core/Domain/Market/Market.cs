using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using RecipeTraderDotNet.Core.Domain.Recipe;
using RecipeTraderDotNet.Core.Domain.Repositories;

namespace RecipeTraderDotNet.Core.Domain.Market
{
    public class Market : IMarket
    {
        private readonly IPublicRecipeRepository _publicRecipeRepo;
        private readonly IMoneyAccountRepository _moneyAccountRepo;

        public Market(IPublicRecipeRepository publicRecipeRepo, IMoneyAccountRepository moneyAccountRepo)
        {
            _publicRecipeRepo = publicRecipeRepo;
            _moneyAccountRepo = moneyAccountRepo;
        }

        public List<PublicRecipe> GetAllRecipes()
        {
            return _publicRecipeRepo.GetAll();
        }

        public bool Publish(PrivateRecipe privateRecipe, decimal price)
        {
            var pubR = new PublicRecipe(privateRecipe, price);
            _publicRecipeRepo.Insert(pubR);
            return true;
        }
      
        public bool TakeDown(int publicRecipeId, string requestUserId)
        {
            var pubR = _publicRecipeRepo.GetById(publicRecipeId);
            if (pubR.Author == requestUserId)
            {
                _publicRecipeRepo.Delete(publicRecipeId);
            }
            
            return true;
        }

        public void Review(int publicRecipeId, UserReview review)
        {
            var pubR = _publicRecipeRepo.GetById(publicRecipeId);
            if (pubR.Reviews == null) pubR.Reviews = new List<UserReview>();
            pubR.Reviews.Add(review);

        }

        public PrivateRecipe Purchase(int publicRecipeId)
        {
            //money transaction should take place
            //todo: system charges money for transaction
            throw new NotImplementedException();
        }

        public SystemInfo GetSystemInfo()
        {
            var info = new SystemInfo();
            var allAccounts = _moneyAccountRepo.GetAll();

            info.TotalUsers = allAccounts.Count;
            info.TotalCurrency = allAccounts.Sum(a => a.Balance);
            return info;
        }
    }
}

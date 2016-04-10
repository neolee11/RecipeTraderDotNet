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

        public string Sell(PrivateRecipe privateRecipe, decimal price)
        {
            //Check if user already sold this recipe
            var existingRecipe = _publicRecipeRepo.GetByUserIdAndTitle(privateRecipe.Author, privateRecipe.Title);
            if (existingRecipe != null) return "You alreayd sold this recipe before";

            var pubR = PublicRecipe.ConvertFromPrivateRecipe(privateRecipe);
            pubR.Price = price;
            pubR.TimePublished = DateTime.UtcNow;

            _publicRecipeRepo.Insert(pubR);
            return string.Empty;
        }
      
        public string TakeDown(int publicRecipeId, string requestUserId)
        {
            var pubR = _publicRecipeRepo.GetById(publicRecipeId);
            if (pubR.Author == requestUserId)
            {
                _publicRecipeRepo.Delete(publicRecipeId);
                return String.Empty;
            }
            else
            {
                return "Cannot take down other people's recipe in the Market";
            }
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="publicRecipeId"></param>
        /// <param name="reviewerUserId"></param>
        /// <param name="rating"></param>
        /// <param name="comment"></param>
        /// <param name="requestUserId"></param>
        /// <returns>Any error message. Empty message indicates success</returns>
        public string Review(int publicRecipeId, string reviewerUserId, int rating, string comment)
        {
            var pubR = _publicRecipeRepo.GetById(publicRecipeId);
            if (pubR.Author == reviewerUserId) return "Recipe author cannot review his/her own recipe";

            var review = new UserReview
            {
               ReviewerUserId = reviewerUserId,
               Rating = rating,
               Comment = comment,
               PublicRecipe = pubR
            };

            pubR.AddReview(review);
            _publicRecipeRepo.Update(pubR);  //Save entire graph of data
            return string.Empty;
        }

        public PrivateRecipe Purchase(int publicRecipeId, string requestUserId)
        {
            var pubR = _publicRecipeRepo.GetById(publicRecipeId);
            var privateRecipe = PrivateRecipe.ConvertFromPublicRecipe(pubR);
            privateRecipe.PurchaseInformation = new RecipePurchaseInformation
            {
                PrivateRecipe = privateRecipe,
                OriginalMarketRecipe = pubR,
                TimePurchased = DateTime.UtcNow
            };

            if (pubR.Author != requestUserId)
            {
                //money transaction takes place
                var purchaserAccount = _moneyAccountRepo.GetUserMoneyAccount(requestUserId);
                var sellerAccount = _moneyAccountRepo.GetUserMoneyAccount(pubR.Author);
                var price = pubR.Price;

                if (purchaserAccount.Balance < price) return null;

                purchaserAccount.Balance -= price;
                sellerAccount.Balance += price;

                _moneyAccountRepo.Update(purchaserAccount);
                _moneyAccountRepo.Update(sellerAccount);

                //todo: system charges money for transaction
            }

            return privateRecipe;
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

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

        public Market(IPublicRecipeRepository publicRecipeRepo)
        {
            _publicRecipeRepo = publicRecipeRepo;
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
            throw new NotImplementedException();
        }
    }
}

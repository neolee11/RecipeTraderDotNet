using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeTraderDotNet.Core.Domain.Recipe;
using RecipeTraderDotNet.Core.Domain.Repositories;

namespace RecipeTraderDotNet.Data.Repositories.Memory
{
    public class PublicRecipeRepository : IPublicRecipeRepository
    {
        public List<PublicRecipe> GetAll()
        {
            throw new NotImplementedException();
        }

        public PublicRecipe GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(PublicRecipe t)
        {
            throw new NotImplementedException();
        }

        public void Update(PublicRecipe t)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public PublicRecipe GetByUserIdAndTitle(string userId, string title)
        {
            throw new NotImplementedException();
        }
    }
}

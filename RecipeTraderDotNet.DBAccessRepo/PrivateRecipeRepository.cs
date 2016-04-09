using System;
using System.Collections.Generic;
using System.Linq;
using RecipeTraderDotNet.Core.Domain.Recipe;
using RecipeTraderDotNet.Data;
using System.Data.Entity;
using RecipeTraderDotNet.Core.Domain.Repositories;

namespace RecipeTraderDotNet.DBAccessRepo
{
    public class PrivateRecipeRepository : IPrivateRecipeRepository
    {
        public List<PrivateRecipe> GetAll()
        {
            throw new NotImplementedException();
        }

        public PrivateRecipe GetById(int id)
        {
            using (var ctx = new RecipeTraderModelContext())
            {
                var recipe = ctx.PrivateRecipes.Include(r => r.Items).FirstOrDefault(r => r.Id == id);
                return recipe;
            }
        }

        public void Insert(PrivateRecipe t)
        {
            using (var ctx = new RecipeTraderModelContext())
            {
                ctx.PrivateRecipes.Add(t);
                ctx.SaveChanges();
            }
        }

        public void Update(PrivateRecipe t)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<PrivateRecipe> GetUserRecipes(string userId)
        {
            throw new NotImplementedException();
        }
    }
}

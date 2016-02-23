using System;
using System.Collections.Generic;
using System.Linq;
using RecipeTraderDotNet.Core.Domain.Entity;
using RecipeTraderDotNet.Core.Domain.Recipe;
using RecipeTraderDotNet.Data;

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
                var recipe = ctx.PrivateRecipes.FirstOrDefault(r => r.Id == id);
                //if (recipe != null)
                //{
                //    ctx.Entry(recipe).Reference(r => r.Items).Load();
                //}

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
    }
}

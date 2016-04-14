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
        private readonly List<PublicRecipe> _currentPublicRecipeState;

        public PublicRecipeRepository(List<PublicRecipe> currentPublicRecipeState)
        {
            _currentPublicRecipeState = currentPublicRecipeState;
        }

        public List<PublicRecipe> GetAll()
        {
            return _currentPublicRecipeState;
        }

        public PublicRecipe GetById(int id)
        {
            return _currentPublicRecipeState.SingleOrDefault(a => a.Id == id);
        }

        public void Insert(PublicRecipe t)
        {
            if (_currentPublicRecipeState.Exists(r => r.Author == t.Author && r.Title == t.Title)) return;

            var random = new Random();

            if (t.Id == default(int)) t.Id = random.Next(1, int.MaxValue);

            if (t.Items != null && t.Items.Count > 0)
            {
                foreach (var recipeItem in t.Items)
                {
                    if (recipeItem.Id == default(int))
                    {
                        recipeItem.Id = random.Next(1, int.MaxValue);
                    }
                }
            }

            if (t.Reviews != null && t.Reviews.Count > 0)
            {
                foreach (var review in t.Reviews)
                {
                    if (review.Id == default(int))
                    {
                        review.Id = random.Next(1, int.MaxValue);
                    }
                }
            }

            _currentPublicRecipeState.Add(t);
        }

        public void Update(PublicRecipe t)
        {
            var existing = _currentPublicRecipeState.SingleOrDefault(a => a.Id == t.Id);

            if (existing != null)
            {
                _currentPublicRecipeState.Remove(existing);
                //_currentPublicRecipeState.Add(t);
                Insert(t);
            }
        }

        public void Delete(int id)
        {
            //should also delete its items and reviews
            var existing = _currentPublicRecipeState.SingleOrDefault(a => a.Id == id);
            if (existing != null) _currentPublicRecipeState.Remove(existing);
        }

        public PublicRecipe GetByUserIdAndTitle(string userId, string title)
        {
            return _currentPublicRecipeState.SingleOrDefault(r => r.Author == userId && r.Title == title);
        }
    }
}

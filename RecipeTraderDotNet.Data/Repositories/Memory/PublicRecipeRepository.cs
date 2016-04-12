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

            _currentPublicRecipeState.Add(t);
        }

        public void Update(PublicRecipe t)
        {
            var existing = _currentPublicRecipeState.SingleOrDefault(a => a.Id == t.Id);

            if (existing != null)
            {
                _currentPublicRecipeState.Remove(existing);
                _currentPublicRecipeState.Add(t);
            }
        }

        public void Delete(int id)
        {
            var existing = _currentPublicRecipeState.SingleOrDefault(a => a.Id == id);
            if (existing != null) _currentPublicRecipeState.Remove(existing);
        }

        public PublicRecipe GetByUserIdAndTitle(string userId, string title)
        {
            return _currentPublicRecipeState.SingleOrDefault(r => r.Author == userId && r.Title == title);
        }
    }
}

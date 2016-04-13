using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeTraderDotNet.Core.Domain.Recipe;
using RecipeTraderDotNet.Core.Domain.Repositories;

namespace RecipeTraderDotNet.Data.Repositories.Memory
{
    public class PrivateRecipeRepository : IPrivateRecipeRepository
    {
        private readonly List<PrivateRecipe> _currentPrivateRecipeState;

        public PrivateRecipeRepository(List<PrivateRecipe> currentPrivateRecipeState)
        {
            _currentPrivateRecipeState = currentPrivateRecipeState;
        }

        public List<PrivateRecipe> GetAll()
        {
            return _currentPrivateRecipeState;
        }

        public PrivateRecipe GetById(int id)
        {
            return _currentPrivateRecipeState.SingleOrDefault(a => a.Id == id);
        }

        public void Insert(PrivateRecipe t)
        {
            var random = new Random();
            t.Id = random.Next(1, Int32.MaxValue);
            _currentPrivateRecipeState.Add(t);
        }

        public void Update(PrivateRecipe t)
        {
            var existing = _currentPrivateRecipeState.SingleOrDefault(a => a.Id == t.Id);

            if (existing != null)
            {
                _currentPrivateRecipeState.Remove(existing);
                _currentPrivateRecipeState.Add(t);
            }
        }

        public void Delete(int id)
        {
            var existing = _currentPrivateRecipeState.SingleOrDefault(a => a.Id == id);
            if (existing != null) _currentPrivateRecipeState.Remove(existing);
        }

        public List<PrivateRecipe> GetUserRecipes(string userId)
        {
            return _currentPrivateRecipeState.Where(r => r.OwnerUserId == userId).ToList();
        }
    }
}


//public async Task<PrivateRecipe> GetByIdAsync(int id)
//{
//    using (var client = new HttpClient())
//    {
//        var baseUrl = "http://localhost:5849/";
//        client.BaseAddress = new Uri(baseUrl);
//        client.DefaultRequestHeaders.Accept.Clear();
//        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

//        var apiUrl = "api/privateRecipe/2";
//        HttpResponseMessage response = await client.GetAsync(apiUrl);
//        if (response.IsSuccessStatusCode)
//        {
//            var json = await response.Content.ReadAsStringAsync();
//            var privateRecipe = JsonConvert.DeserializeObject<PrivateRecipe>(json);
//            return privateRecipe;
//        }

//        return null;
//    }
//}
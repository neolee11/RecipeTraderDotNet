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
        public List<PrivateRecipe> GetAll()
        {
            throw new NotImplementedException();
        }

        public PrivateRecipe GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(PrivateRecipe t)
        {
            throw new NotImplementedException();
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RecipeTraderDotNet.Core.Domain.Recipe;

namespace RecipeTraderDotNet.ApiAccessRepo
{
    public class PrivateRecipeRepository
    {
        public async Task<PrivateRecipe> GetByIdAsync(int id)
        {
            using (var client = new HttpClient())
            {
                var baseUrl = "http://localhost:5849/";
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var apiUrl = "api/privateRecipe/2";
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var privateRecipe = JsonConvert.DeserializeObject<PrivateRecipe>(json);
                    return privateRecipe;
                }

                return null;
            }
        }
    }
}

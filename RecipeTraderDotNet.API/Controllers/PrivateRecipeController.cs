using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RecipeTraderDotNet.Core.Domain.Entity;
using RecipeTraderDotNet.DBAccessRepo;

namespace RecipeTraderDotNet.API.Controllers
{
    [RoutePrefix("api/privateRecipe")]
    public class PrivateRecipeController : ApiController
    {
        private IPrivateRecipeRepository _privateRecipeRepo;

        public PrivateRecipeController() : this(new PrivateRecipeRepository())
        {
        }

        public PrivateRecipeController(IPrivateRecipeRepository privateRecipeRepo)
        {
            this._privateRecipeRepo = privateRecipeRepo;
        }

        [Route("{recipeId}")]
        public IHttpActionResult GetRecipeById(int recipeId)
        {
            var result = _privateRecipeRepo.GetById(recipeId);
            return Ok(result);
        }

        [Route("testjson")]
        [HttpGet]
        public IHttpActionResult TestJson()
        {
            dynamic test = new
            {
                Name = "Daniel",
                age = 30
            };

            return Ok(test);
        }


    }
}

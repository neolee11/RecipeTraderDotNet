using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecipeTraderDotNet.Core.Domain.Recipe;
using RecipeTraderDotNet.Core.Domain.Repositories;
using RecipeTraderDotNet.DBAccessRepo;

namespace RecipeTraderDotNet.Web.Controllers
{
    public class HomeController : Controller
    {
        private IPrivateRecipeRepository privateRecipeRepository = new PrivateRecipeRepository();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public void InsertRecipe()
        {
            var recipe = new PrivateRecipe();

            var item1= new RecipeItem {Description = "Step 1"};
            var item2= new RecipeItem {Description = "Step 2"};
            recipe.Add(item1);
            recipe.Add(item2);

            privateRecipeRepository.Insert(recipe);
        }

        public JsonResult GetRecipe(int recipeId)
        {
            var result = privateRecipeRepository.GetById(recipeId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
namespace CookIt.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookIt.Services.Data;
    using CookIt.Web.ViewModels.Recipe;
    using Microsoft.AspNetCore.Mvc;

    public class RecipesController : BaseController
    {
        private readonly IRecipeService recipeService;

        public RecipesController(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
        }

        public IActionResult Details(int id)
        {
            var recipe = this.recipeService.GetRecipeWithoutDeleted<RecipeDetailsViewModel>(id);

            return this.View(recipe);
        }
    }
}

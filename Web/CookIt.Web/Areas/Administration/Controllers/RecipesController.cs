namespace CookIt.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CookIt.Services;
    using CookIt.Services.Data;
    using CookIt.Web.ViewModels.Ingridient;
    using CookIt.Web.ViewModels.Recipe;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class RecipesController : AdministrationController
    {
        private readonly ICloudinaryService cloudinaryService;
        private readonly IIngredientService ingredientService;

        public RecipesController(
            ICloudinaryService cloudinaryService,
            IIngredientService ingredientService)
        {
            this.cloudinaryService = cloudinaryService;
            this.ingredientService = ingredientService;
        }

        [HttpGet]
        public async Task<IActionResult> CreateRecipe()
        {
            var ingredients = await this.ingredientService.GetAllIngreients<AllIngredientViewModel>().ToListAsync();
            var model = new RecipeModel { Ingredients = ingredients};
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecipe(RecipeModel recipeBindingModel)
        {
            if (!this.ModelState.IsValid)
            {
                var ingredients = await this.ingredientService.GetAllIngreients<AllIngredientViewModel>().ToListAsync();
                recipeBindingModel.Ingredients = ingredients;
                return this.View(recipeBindingModel);
            }
            var a = 5;
            return this.View();
        }
    }
}

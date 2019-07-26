namespace CookIt.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookIt.Services;
    using CookIt.Services.Data;
    using CookIt.Web.ViewModels.Ingridient;
    using CookIt.Web.ViewModels.Recipe;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class RecipesController : AdministrationController
    {
        private const string ErrorMessageRecipeNameAlreadyExists = "Recipe with name {0} already exists.";
        private const string ErrorMessageInvalidIngredients = "Invalid Ingredients.";

        private readonly ICloudinaryService cloudinaryService;
        private readonly IIngredientService ingredientService;
        private readonly IRecipeService recipeService;

        public RecipesController(
            ICloudinaryService cloudinaryService,
            IIngredientService ingredientService,
            IRecipeService recipeService)
        {
            this.cloudinaryService = cloudinaryService;
            this.ingredientService = ingredientService;
            this.recipeService = recipeService;
        }

        [HttpGet]
        public async Task<IActionResult> CreateRecipe()
        {
            var ingredients = await this.ingredientService.GetAllIngreients<AllIngredientViewModel>().ToListAsync();
            var model = new RecipeModel { Ingredients = ingredients };
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

            bool areAllIdsValid = await this.ingredientService.CheckExistingIngredientId(
                recipeBindingModel.InputModel.Ingredients
                .Select(x => x.IngredientId).ToList());

            if (!areAllIdsValid)
            {
                var ingredients = await this.ingredientService.GetAllIngreients<AllIngredientViewModel>().ToListAsync();
                recipeBindingModel.Ingredients = ingredients;
                this.ModelState.AddModelError(string.Empty, ErrorMessageInvalidIngredients);
                return this.View(recipeBindingModel);
            }

            if (this.recipeService.CheckRecipeByName(recipeBindingModel.InputModel.Name))
            {
                var ingredients = await this.ingredientService.GetAllIngreients<AllIngredientViewModel>().ToListAsync();
                recipeBindingModel.Ingredients = ingredients;
                this.ModelState.AddModelError(string.Empty, string.Format(ErrorMessageRecipeNameAlreadyExists, recipeBindingModel.InputModel.Name));
                return this.View(recipeBindingModel);
            }

            var imageUrl = await this.cloudinaryService.UploadImageAsync(recipeBindingModel.InputModel.Image, recipeBindingModel.InputModel.Name);
            await this.recipeService.CreateRecipe<RecipeCreateBindingModel>(recipeBindingModel.InputModel, imageUrl);

            return this.RedirectToAction("AllRecipes", "Recipes");
        }

        [HttpGet]
        public async Task<IActionResult> AllRecipes()
        {
            var recipes = await this.recipeService.GetAllRecipes<RecipeAllViewModel>().ToListAsync();

            return this.View(recipes);
        }
    }
}

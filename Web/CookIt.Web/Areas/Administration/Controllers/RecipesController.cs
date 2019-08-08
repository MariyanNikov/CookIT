namespace CookIt.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookIt.Services;
    using CookIt.Services.Data;
    using CookIt.Web.BindingModels.Recipe;
    using CookIt.Web.ViewModels.Ingridient;
    using CookIt.Web.ViewModels.Recipe;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using X.PagedList;

    public class RecipesController : AdministrationController
    {
        private const string ErrorMessageRecipeNameAlreadyExists = "Recipe with name {0} already exists.";
        private const string ErrorMessageInvalidIngredients = "Invalid Ingredients.";
        private const string ErrorMessageInvalidRecipeId = "invalid Recipe with id {0}.";
        private const string SuccessMessageCreatedRecipe = "You have successfully created {0} recipe.";
        private const string SuccessMessageSoftDeleteRecipe = "You have successfully softly deleted recipe with id {0}.";
        private const string SuccessMessageUnDeleteRecipe = "You have successfully undeleted recipe with id {0}.";
        private const string SuccessMessageEditRecipe = "You have successfully edited {0} recipe.";

        private const int DefaultPageSize = 10;
        private const int DefaultPage = 1;

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

            bool areAllIdsValid = this.ingredientService.CheckExistingIngredientId(
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
            this.TempData["StatusMessage"] = string.Format(SuccessMessageCreatedRecipe, recipeBindingModel.InputModel.Name);
            return this.RedirectToAction("AllRecipes", "Recipes");
        }

        [HttpGet]
        public async Task<IActionResult> AllRecipes(int? p)
        {
            var page = p ?? DefaultPage;

            var recipes = await this.recipeService.GetAllRecipesWithDeleted<RecipeAllViewModel>().ToListAsync();

            var pageRecipes = recipes.ToPagedList(page, DefaultPageSize);

            return this.View(pageRecipes);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            if (!this.recipeService.CheckRecipeById(id))
            {
                this.TempData["StatusMessage"] = string.Format(ErrorMessageInvalidRecipeId, id);
                return this.RedirectToAction("AllRecipes", "Recipes");
            }

            await this.recipeService.SoftDeleteRecipe(id);
            this.TempData["StatusMessage"] = string.Format(SuccessMessageSoftDeleteRecipe, id);
            return this.RedirectToAction("AllRecipes", "Recipes");
        }

        [HttpPost]
        public async Task<IActionResult> UnDelete(int id)
        {
            if (!this.recipeService.CheckRecipeById(id))
            {
                this.TempData["StatusMessage"] = string.Format(ErrorMessageInvalidRecipeId, id);
                return this.RedirectToAction("AllRecipes", "Recipes");
            }

            await this.recipeService.UnDeleteRecipe(id);
            this.TempData["StatusMessage"] = string.Format(SuccessMessageUnDeleteRecipe, id);
            return this.RedirectToAction("AllRecipes", "Recipes");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var recipe = this.recipeService.FindRecipeById<RecipeEditBindingModel>(id);
            var ingredients = await this.ingredientService.GetAllIngreients<AllIngredientViewModel>().ToListAsync();
            var model = new RecipeEditBaseModel { Ingredients = ingredients, InputModel = recipe };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RecipeEditBaseModel recipeBindingModel)
        {
            if (!this.ModelState.IsValid)
            {
                var ingredients = await this.ingredientService.GetAllIngreients<AllIngredientViewModel>().ToListAsync();
                recipeBindingModel.Ingredients = ingredients;
                return this.View(recipeBindingModel);
            }

            bool areAllIdsValid = this.ingredientService.CheckExistingIngredientId(
                recipeBindingModel.InputModel.Ingredients
                .Select(x => x.IngredientId).ToList());

            if (!areAllIdsValid)
            {
                var ingredients = await this.ingredientService.GetAllIngreients<AllIngredientViewModel>().ToListAsync();
                recipeBindingModel.Ingredients = ingredients;
                this.ModelState.AddModelError(string.Empty, ErrorMessageInvalidIngredients);
                return this.View(recipeBindingModel);
            }

            if (recipeBindingModel.InputModel.ImageUpload != null)
            {
                var imageUrl = await this.cloudinaryService.UploadImageAsync(recipeBindingModel.InputModel.ImageUpload, recipeBindingModel.InputModel.Name);
                recipeBindingModel.InputModel.Image = imageUrl;
            }

            recipeBindingModel.InputModel.Id = recipeBindingModel.Id;
            await this.recipeService.UpdateRecipe<RecipeEditBindingModel>(recipeBindingModel.InputModel, recipeBindingModel.Id);

            // TODO: Renaming to existing name check.
            this.TempData["StatusMessage"] = string.Format(SuccessMessageEditRecipe, recipeBindingModel.InputModel.Name);
            return this.RedirectToAction("AllRecipes", "Recipes");
        }
    }
}

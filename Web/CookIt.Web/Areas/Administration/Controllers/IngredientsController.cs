namespace CookIt.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CookIt.Services.Data;
    using CookIt.Web.BindingModels.Ingredient;
    using CookIt.Web.ViewModels.Ingridient;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using X.PagedList;

    public class IngredientsController : AdministrationController
    {
        private const string ErrorMessageIngredientTypeAlreadyExists = "Ingredient Type with name: {0} already exists.";
        private const string ErrorMessageIngredientTypeDoesNotExist = "Ingredient Type with name: {0} does not exist.";
        private const string ErrorMessageInvalidIngredientType = "Invalid ingredient type.";
        private const string ErrorMessageIngredientDoesNotExist = "Ingredient with name: {0} does not exist.";
        private const string ErrorMessageIngredientAlreadyExists = "Ingredient with name: {0} already exists.";
        private const string ErrorMessageIngredientTypeAlreadyUsed = "You cannot delete Ingedient Type which is already been used in Ingredient.";
        private const string ErrorMessageIngredientAlreadyUsed = "You cannot delete Ingedient which is already been used in Recipe.";
        private const string SuccessMessageCreateIngredientType = "You have successfully created {0} ingredient type.";
        private const string SuccessMessageRemoveIngredientType = "You have successfully removed {0} ingredient type.";
        private const string SuccessMessageCreateIngredient = "You have successfully created {0} ingredient.";
        private const string SuccessMessageRemoveIngredient = "You have successfully removed {0} ingredient.";

        private const int DefaultPageSize = 10;
        private const int DefaultPage = 1;

        private readonly IIngredientService ingredientService;

        public IngredientsController(IIngredientService ingredientService)
        {
            this.ingredientService = ingredientService;
        }

        [HttpGet]
        public async Task<IActionResult> AllIngredientTypes(int? p)
        {
            var page = p ?? DefaultPage;

            var ingredientTypes = await this.ingredientService.GetAllIngreientTypes<AllIngredientTypeViewModel>().ToListAsync();

            var pageIngredientTypes = ingredientTypes.ToPagedList(page, DefaultPageSize);

            return this.View(pageIngredientTypes);
        }

        [HttpGet]
        public IActionResult CreateIngredientType()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateIngredientType(IngredientTypeBindingModel ingredientTypeModel)
        {
            bool isIngredientFromDb = this.ingredientService.CheckIfIngredientTypeExistByName(ingredientTypeModel.Name);
            if (isIngredientFromDb)
            {
                this.ModelState.AddModelError(string.Empty, string.Format(ErrorMessageIngredientTypeAlreadyExists, ingredientTypeModel.Name));
                return this.View(ingredientTypeModel);
            }

            await this.ingredientService.CreateIngredientType(ingredientTypeModel.Name);
            this.TempData["StatusMessage"] = string.Format(SuccessMessageCreateIngredientType, ingredientTypeModel.Name);
            return this.RedirectToAction("AllIngredientTypes", "Ingredients");
        }

        [HttpPost(Name = "RemoveIngredientType")]
        public async Task<IActionResult> RemoveIngredientType([FromRoute]int id)
        {
            var ingredientName = this.ingredientService.GetIngredientTypeNameById(id);

            if (ingredientName == null)
            {
                this.TempData["StatusMessage"] = string.Format(ErrorMessageIngredientTypeDoesNotExist, ingredientName);
                return this.RedirectToAction("AllIngredientTypes", "Ingredients");
            }

            if (this.ingredientService.HasIngredientWithType(id))
            {
                this.TempData["StatusMessage"] = ErrorMessageIngredientTypeAlreadyUsed;
                return this.RedirectToAction("AllIngredientTypes", "Ingredients");
            }

            await this.ingredientService.RemoveIngredientTypeById(id);

            this.TempData["StatusMessage"] = string.Format(SuccessMessageRemoveIngredientType, ingredientName);
            return this.RedirectToAction("AllIngredientTypes", "Ingredients");
        }

        [HttpGet]
        public IActionResult CreateIngredient()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateIngredient(IngredientBindingModel ingredientModel)
        {
            if (!this.ingredientService.CheckIfIngredientTypeExistById(ingredientModel.IngredientTypeId))
            {
                this.ModelState.AddModelError(string.Empty, ErrorMessageInvalidIngredientType);
                return this.View();
            }

            bool isIngredientFromDb = this.ingredientService.IngredientExistsByName(ingredientModel.Name);
            if (isIngredientFromDb)
            {
                this.ModelState.AddModelError(string.Empty, string.Format(ErrorMessageIngredientAlreadyExists, ingredientModel.Name));
                return this.View(ingredientModel);
            }

            await this.ingredientService.CreateIngredient<IngredientBindingModel>(ingredientModel);

            this.TempData["StatusMessage"] = string.Format(SuccessMessageCreateIngredient, ingredientModel.Name);
            return this.RedirectToAction("AllIngredients", "Ingredients");
        }

        [HttpGet]
        public async Task<IActionResult> AllIngredients(int? p)
        {
            var page = p ?? DefaultPage;

            var ingredients = await this.ingredientService.GetAllIngreients<AllIngredientViewModel>().ToListAsync();

            var pageIngredients = ingredients.ToPagedList(page, DefaultPageSize);

            return this.View(pageIngredients);
        }

        [HttpPost(Name = "RemoveIngredient")]
        public async Task<IActionResult> RemoveIngredient([FromRoute]int id)
        {
            var ingredientName = this.ingredientService.GetIngredientNameById(id);

            if (ingredientName == null)
            {
                this.TempData["StatusMessage"] = string.Format(ErrorMessageIngredientDoesNotExist, ingredientName);
                return this.RedirectToAction("AllIngredients", "Ingredients");
            }

            if (this.ingredientService.HasRecipeWithIngredient(id))
            {
                this.TempData["StatusMessage"] = ErrorMessageIngredientAlreadyUsed;
                return this.RedirectToAction("AllIngredients", "Ingredients");
            }

            await this.ingredientService.RemoveIngredientById(id);

            this.TempData["StatusMessage"] = string.Format(SuccessMessageRemoveIngredient, ingredientName);
            return this.RedirectToAction("AllIngredients", "Ingredients");
        }
    }
}

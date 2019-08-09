namespace CookIt.Web.Controllers
{
    using System.Threading.Tasks;

    using CookIt.Data.Models;
    using CookIt.Services.Data;
    using CookIt.Web.ViewModels.Recipe;
    using CookIt.Web.ViewModels.Review;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class RecipesController : BaseController
    {
        private readonly IRecipeService recipeService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IReviewService reviewService;

        public RecipesController(
            IRecipeService recipeService,
            UserManager<ApplicationUser> userManager,
            IReviewService reviewService)
        {
            this.recipeService = recipeService;
            this.userManager = userManager;
            this.reviewService = reviewService;
        }

        public IActionResult Details(int id)
        {
            var recipe = this.recipeService.GetRecipeWithoutDeleted<RecipeDetailsViewModel>(id);

            if (recipe == null)
            {
                return this.Redirect("/");
            }

            var userId = this.userManager.GetUserId(this.User);
            recipe.HasReviewed = this.reviewService.HasReviewedRecipeByUserId(id, userId);

            return this.View(recipe);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(ReviewBindingModel reviewBindingModel, int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.Details), new { id });
            }

            var userId = this.userManager.GetUserId(this.User);
            await this.reviewService.AddAsync<ReviewBindingModel>(reviewBindingModel, id, userId);

            return this.RedirectToAction(nameof(this.Details), new { id });
        }
    }
}

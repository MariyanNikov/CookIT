namespace CookIt.Web.Infrastructure.ViewComponents
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookIt.Services.Data;
    using CookIt.Web.ViewModels.Recipe;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class BestRatingsViewComponent : ViewComponent
    {
        private readonly IRecipeService recipeService;

        public BestRatingsViewComponent(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int amountOfRecipes)
        {
            var recipes = await this.recipeService.GetHighestRatingsRecipes<RecipeBestRatingsViewModel>().Take(amountOfRecipes).ToListAsync();

            return this.View(recipes);
        }
    }
}

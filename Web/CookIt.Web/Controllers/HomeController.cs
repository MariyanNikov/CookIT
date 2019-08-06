namespace CookIt.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookIt.Data.Models;
    using CookIt.Services.Data;
    using CookIt.Services.Data.ApplicationUser;
    using CookIt.Web.ViewModels.Recipe;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using X.PagedList;

    public class HomeController : BaseController
    {
        private const int DefaultPageSize = 8;
        private const int DefaultPages = 1;

        private readonly IRecipeService recipeService;

        public HomeController(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
        }

        public async Task<IActionResult> Index(int? p, string search)
        {
            var page = p ?? DefaultPages;

            var recipes = this.recipeService.GetAllRecipesWithoutDeleted<RecipeIndexViewModel>();

            if (search != null)
            {
               recipes = recipes.Where(x => x.Name.ToLower().Contains(search.ToLower()));
            }

            var pagedRecipes = await recipes.ToPagedListAsync(page, DefaultPageSize);
            this.ViewData["Search"] = search;
            return this.View(pagedRecipes);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => this.View();
    }
}

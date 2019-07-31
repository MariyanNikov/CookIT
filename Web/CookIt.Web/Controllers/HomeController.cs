namespace CookIt.Web.Controllers
{
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

        private readonly IApplicationUserService applicationUserService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRecipeService recipeService;

        public HomeController(
            IApplicationUserService applicationUserService,
            UserManager<ApplicationUser> userManager,
            IRecipeService recipeService)
        {
            this.applicationUserService = applicationUserService;
            this.userManager = userManager;
            this.recipeService = recipeService;
        }

        public async Task<IActionResult> Index(int? p)
        {
            var page = p ?? DefaultPages;

            var recipes = await this.recipeService.GetAllRecipesWithoutDeleted<RecipeIndexViewModel>().ToListAsync();

            var pagedRecipes = recipes.ToPagedList(page, DefaultPageSize);

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

namespace CookIt.Web.Controllers
{
    using CookIt.Data.Models;
    using CookIt.Services.Data.ApplicationUser;
    using CookIt.Web.ViewModels.Address;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IApplicationUserService applicationUserService;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(IApplicationUserService applicationUserService, UserManager<ApplicationUser> userManager)
        {
            this.applicationUserService = applicationUserService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => this.View();
    }
}

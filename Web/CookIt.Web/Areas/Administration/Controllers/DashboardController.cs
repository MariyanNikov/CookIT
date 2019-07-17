namespace CookIt.Web.Areas.Administration.Controllers
{
    using CookIt.Common;
    using CookIt.Services.Data;
    using CookIt.Services.Data.ApplicationUser;
    using CookIt.Web.Areas.Administration.ViewModels.Dashboard;
    using CookIt.Web.ViewModels.Administration;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService settingsService;
        private readonly IApplicationUserService applicationUserService;

        public DashboardController(ISettingsService settingsService, IApplicationUserService applicationUserService)
        {
            this.settingsService = settingsService;
            this.applicationUserService = applicationUserService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel { SettingsCount = this.settingsService.GetCount(), };
            return this.View(viewModel);
        }

        public IActionResult Admins()
        {
            var admins = this.applicationUserService.GetUsersByRoleName<AdminViewModel>(GlobalConstants.AdministratorRoleName);
            return this.View(admins);
        }
    }
}

namespace CookIt.Web.Infrastructure.ViewComponents
{
    using System.Security.Claims;

    using CookIt.Services.Data.ApplicationUser;
    using CookIt.Web.ViewModels.Address;
    using Microsoft.AspNetCore.Mvc;

    public class CheckoutAddressesViewComponent : ViewComponent
    {
        private readonly IApplicationUserService applicationUserService;

        public CheckoutAddressesViewComponent(IApplicationUserService applicationUserService)
        {
            this.applicationUserService = applicationUserService;
        }

        public IViewComponentResult Invoke()
        {
            var userId = this.UserClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var addresses = this.applicationUserService.GetAllAddresses<AddressViewModel>(userId);

            return this.View(addresses);
        }
    }
}

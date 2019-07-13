namespace CookIt.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookIt.Data.Models;
    using CookIt.Services.Data.ApplicationUser;
    using CookIt.Web.ViewModels.Address;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

#pragma warning disable SA1649 // File name should match first type name
    public class AddressesModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IApplicationUserService applicationUserService;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AddressesModel(
            UserManager<ApplicationUser> userManager,
            IApplicationUserService applicationUserService,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.applicationUserService = applicationUserService;
            this.signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public AddressBindingModel Input { get; set; }

        [BindProperty]
        public IEnumerable<AddressViewModel> Addresses { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            this.Addresses = this.applicationUserService.GetAllAddresses<AddressViewModel>(user.Id);

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            await this.applicationUserService.AddAddress<AddressBindingModel>(this.Input, user.Id);

            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Added new address.";
            return this.RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAddressAsync(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var addresses = this.applicationUserService.GetAllAddresses<AddressViewModel>(user.Id).Select(x => x.Id);

            if (!addresses.Contains(id))
            {
                return this.NotFound($"Unable to find address with ID '{id}'.");
            }

            await this.applicationUserService.RemoveAddressById(id);

            this.StatusMessage = "Removed address.";
            return this.RedirectToPage();
        }
    }
}

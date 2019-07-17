namespace CookIt.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CookIt.Common;
    using CookIt.Data.Models;
    using CookIt.Services.Data.ApplicationUser;
    using CookIt.Web.ViewModels.Administration;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : AdministrationController
    {
        private const string ErrorMessageEmailDoesNotExist = "User with email: {0} does not exist.";
        private const string ErrorMessageUserAlreadyAdmin = "User with email: {0} is already an Admin.";
        private const string ErrorMessageUserAlreadyCourier = "User with email: {0} is already an Courier.";
        private const string ErrorMessageAdminIdDoesNotExist = "Error: Admin with such ID does not exist.";
        private const string ErrorMessageCourierIdDoesNotExist = "Error: Courier with such ID does not exist.";
        private const string ErrorMessageInsufficientPermissions = "Error: Insufficient permissions.";
        private const string SuccessMessageMakeAdmin = "You have successfully made {0} an admin.";
        private const string SuccessMessageMakeCourier = "You have successfully made {0} an courier.";
        private const string SuccessMessageRemoveAdmin = "You have successfully removed {0} from the administration.";
        private const string SuccessMessageRemoveCourier = "You have successfully removed {0} from the couriers.";

        private readonly IApplicationUserService applicationUserService;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(IApplicationUserService applicationUserService, UserManager<ApplicationUser> userManager)
        {
            this.applicationUserService = applicationUserService;
            this.userManager = userManager;
        }

        [HttpGet(Name = "Admins")]
        public async Task<IActionResult> Admins()
        {
            var admins = await this.applicationUserService.GetUsersByRoleName<AdminViewModel>(GlobalConstants.AdministratorRoleName);

            return this.View(admins);
        }

        [HttpPost(Name = "Admins")]
        public async Task<IActionResult> Admins([FromForm]string email)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            if (user == null)
            {
                this.ModelState.AddModelError(string.Empty, string.Format(ErrorMessageEmailDoesNotExist, email));
                var admins = await this.applicationUserService.GetUsersByRoleName<AdminViewModel>(GlobalConstants.AdministratorRoleName);
                return this.View(admins);
            }
            else if (await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName))
            {
                this.ModelState.AddModelError(string.Empty, string.Format(ErrorMessageUserAlreadyAdmin, email));
                var admins = await this.applicationUserService.GetUsersByRoleName<AdminViewModel>(GlobalConstants.AdministratorRoleName);
                return this.View(admins);
            }

            await this.userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
            this.TempData["StatusMessage"] = string.Format(SuccessMessageMakeAdmin, email);

            return this.Redirect("Admins");
        }

        [HttpPost(Name = "RemoveAdmin")]
        public async Task<IActionResult> RemoveAdmin([FromRoute]string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            // TODO:  root@root.com
            if (user == null)
            {
                this.TempData["StatusMessage"] = ErrorMessageAdminIdDoesNotExist;
                return this.RedirectToAction("Admins", "Users");
            }
            else if (user.Email == "root@root.com")
            {
                this.TempData["StatusMessage"] = ErrorMessageInsufficientPermissions;
                return this.RedirectToAction("Admins", "Users");
            }

            await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.AdministratorRoleName);
            this.TempData["StatusMessage"] = string.Format(SuccessMessageRemoveAdmin, user.Email);
            return this.RedirectToAction("Admins", "Users");
        }

        [HttpGet(Name = "Couriers")]
        public async Task<IActionResult> Couriers()
        {
            var couriers = await this.applicationUserService.GetUsersByRoleName<CourierViewModel>(GlobalConstants.CourierRoleName);

            return this.View(couriers);
        }

        [HttpPost(Name = "Couriers")]
        public async Task<IActionResult> Couriers([FromForm]string email)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            if (user == null)
            {
                this.ModelState.AddModelError(string.Empty, string.Format(ErrorMessageEmailDoesNotExist, email));
                var couriers = await this.applicationUserService.GetUsersByRoleName<CourierViewModel>(GlobalConstants.CourierRoleName);
                return this.View(couriers);
            }
            else if (await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName))
            {
                this.ModelState.AddModelError(string.Empty, string.Format(ErrorMessageUserAlreadyCourier, email));
                var couriers = await this.applicationUserService.GetUsersByRoleName<CourierViewModel>(GlobalConstants.CourierRoleName);
                return this.View(couriers);
            }

            await this.userManager.AddToRoleAsync(user, GlobalConstants.CourierRoleName);
            this.TempData["StatusMessage"] = string.Format(SuccessMessageMakeCourier, email);

            return this.Redirect("Couriers");
        }

        [HttpPost(Name = "RemoveCourier")]
        public async Task<IActionResult> RemoveCourier([FromRoute]string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                this.TempData["StatusMessage"] = ErrorMessageCourierIdDoesNotExist;
                return this.RedirectToAction("Couriers", "Users");
            }

            await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.CourierRoleName);
            this.TempData["StatusMessage"] = string.Format(SuccessMessageRemoveCourier, user.Email);
            return this.RedirectToAction("Couriers", "Users");
        }
    }
}

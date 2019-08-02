namespace CookIt.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using CookIt.Data.Models;
    using CookIt.Services.Data;
    using CookIt.Web.ViewModels.Order;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly UserManager<ApplicationUser> userManager;

        public OrdersController(
            IOrderService orderService,
            IShoppingCartService shoppingCartService,
            UserManager<ApplicationUser> userManager)
        {
            this.orderService = orderService;
            this.shoppingCartService = shoppingCartService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Checkout()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!this.shoppingCartService.HasItemsInCart(userId))
            {
                return this.Redirect("/");
            }
            var model = new CheckoutInputModel();
            model.FullName = this.User.FindFirstValue("FullName");
            var user = await this.userManager.GetUserAsync(this.User);
            model.PhoneNumber = await this.userManager.GetPhoneNumberAsync(user);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutInputModel checkoutBindingModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(checkoutBindingModel);
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            checkoutBindingModel.IssuerId = userId;

            await this.orderService.Checkout<CheckoutInputModel>(checkoutBindingModel);

            // TODO: Clear the cart

            return this.Redirect("/");
        }
    }
}

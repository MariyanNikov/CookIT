namespace CookIt.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookIt.Common;
    using CookIt.Data.Models;
    using CookIt.Services.Data;
    using CookIt.Web.BindingModels.Order;
    using CookIt.Web.ViewModels.Order;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

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

            var model = new CheckoutBindingModel();
            model.FullName = this.User.FindFirstValue("FullName");
            var user = await this.userManager.GetUserAsync(this.User);
            model.PhoneNumber = await this.userManager.GetPhoneNumberAsync(user);
            model.DeliveryDate = DateTime.UtcNow.ToLocalTime();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutBindingModel checkoutBindingModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(checkoutBindingModel);
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            checkoutBindingModel.IssuerId = userId;

            var pendingStatus = this.orderService.GetOrderStatusByName(GlobalConstants.PendingOrderStatus);
            checkoutBindingModel.OrderStatusId = pendingStatus.Id;

            checkoutBindingModel.IssuedOn = DateTime.UtcNow;

            var shoppingCartItems = await this.shoppingCartService.CheckOutGetCartItems(userId);
            checkoutBindingModel.TotalPrice = shoppingCartItems.Sum(x => x.Recipe.Price);

            await this.orderService.Checkout<CheckoutBindingModel>(checkoutBindingModel, userId);
            await this.shoppingCartService.ClearShoppingCart(userId);

            return this.Redirect("/");
        }

        public async Task<IActionResult> Details(string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!this.User.IsInRole(GlobalConstants.AdministratorRoleName) &&
                !this.User.IsInRole(GlobalConstants.CourierRoleName) &&
                !this.orderService.HasOrderWithId(userId, id))
            {
                return this.Redirect("/");
            }

            var order = await this.orderService.FindOrderById<OrderDetailsViewModel>(id).SingleOrDefaultAsync();

            return this.View(order);
        }

        public async Task<IActionResult> MyOrders()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var orders = await this.orderService.GetAllOrdersByUserId<MyOrdersViewModel>(userId).ToListAsync();

            return this.View(orders);
        }
    }
}

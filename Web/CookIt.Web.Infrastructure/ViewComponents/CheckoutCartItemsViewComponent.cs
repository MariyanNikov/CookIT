namespace CookIt.Web.Infrastructure.ViewComponents
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookIt.Services.Data;
    using CookIt.Web.ViewModels.ShoppingCart;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class CheckoutCartItemsViewComponent : ViewComponent
    {
        private readonly IShoppingCartService shoppingCartService;

        public CheckoutCartItemsViewComponent(IShoppingCartService shoppingCartService)
        {
            this.shoppingCartService = shoppingCartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = this.UserClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cartItems = await this.shoppingCartService.GetAllShoppingCartItems<CartItemsViewModel>(userId).ToListAsync();

            return this.View(cartItems);
        }
    }
}

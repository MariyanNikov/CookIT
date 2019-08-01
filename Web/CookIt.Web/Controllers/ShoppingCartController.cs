namespace CookIt.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookIt.Services.Data;
    using CookIt.Web.ViewModels.ShoppingCart;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize]
    public class ShoppingCartController : BaseController
    {
        private readonly IShoppingCartService shoppingCartService;

        public ShoppingCartController(
            IShoppingCartService shoppingCartService)
        {
            this.shoppingCartService = shoppingCartService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var cartItems = await this.shoppingCartService.GetAllShoppingCartItems<CartItemsViewModel>(userId).ToListAsync();

            return this.View(cartItems);
        }

        public async Task<IActionResult> AddToCart(int id, string returnUrl)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            returnUrl = returnUrl ?? "/";
            if (this.shoppingCartService.IsInShoppingCart(userId, id))
            {
                return this.Redirect(returnUrl);
            }

            await this.shoppingCartService.AddShoppingCartItem(userId, id);

            this.TempData["CartItemAdded"] = true;
            return this.Redirect(returnUrl);
        }

        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await this.shoppingCartService.RemoveShoppingCartItem(userId, id);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}

namespace CookIt.Web.Infrastructure.ViewComponents
{
    using System.Security.Claims;

    using CookIt.Services.Data;
    using Microsoft.AspNetCore.Mvc;

    public class ShoppingCartIconViewComponent : ViewComponent
    {
        private const int DefaultShoppingCartItemsCount = 0;

        private readonly IShoppingCartService shoppingCartService;

        public ShoppingCartIconViewComponent(
            IShoppingCartService shoppingCartService)
        {
            this.shoppingCartService = shoppingCartService;
        }

        public IViewComponentResult Invoke()
        {
            var userId = this.UserClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return this.View(DefaultShoppingCartItemsCount);
            }

            var shoppingItemsCount = this.shoppingCartService.GetShoppingCartItemsCount(userId);
            return this.View(shoppingItemsCount);
        }
    }
}

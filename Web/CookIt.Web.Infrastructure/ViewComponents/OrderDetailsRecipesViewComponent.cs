namespace CookIt.Web.Infrastructure.ViewComponents
{
    using System.Threading.Tasks;

    using CookIt.Services.Data;
    using CookIt.Web.ViewModels.Order;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class OrderDetailsRecipesViewComponent : ViewComponent
    {
        private readonly IOrderService orderService;

        public OrderDetailsRecipesViewComponent(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string orderId)
        {
            var recipes = await this.orderService.GetRecipesFromOrder<OrderDetailsRecipesViewModel>(orderId).ToListAsync();

            return this.View(recipes);
        }
    }
}

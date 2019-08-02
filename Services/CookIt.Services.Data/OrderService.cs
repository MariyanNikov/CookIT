namespace CookIt.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using CookIt.Common;
    using CookIt.Data.Common.Repositories;
    using CookIt.Data.Models;

    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> orderRepository;
        private readonly IRepository<OrderStatus> orderStatusRepository;
        private readonly IShoppingCartService shoppingCartService;

        public OrderService(
            IRepository<Order> orderRepository,
            IRepository<OrderStatus> orderStatusRepository,
            IShoppingCartService shoppingCartService)
        {
            this.orderRepository = orderRepository;
            this.orderStatusRepository = orderStatusRepository;
            this.shoppingCartService = shoppingCartService;
        }

        public async Task<bool> AddOrderStatus(OrderStatus status)
        {
            if (this.GetOrderStatusByName(status.Name) != null)
            {
                return false;
            }

            await this.orderStatusRepository.AddAsync(status);
            await this.orderStatusRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Checkout<TModel>(TModel order)
        {
            var orderForDb = Mapper.Map<Order>(order);
            var pendingStatus = this.GetOrderStatusByName(GlobalConstants.PendingOrderStatus);

            orderForDb.OrderStatusId = pendingStatus.Id;
            orderForDb.IssuedOn = DateTime.UtcNow;

            var shoppingCartItems = await this.shoppingCartService.CheckOutGetCartItems(orderForDb.IssuerId);
            orderForDb.TotalPrice = shoppingCartItems.Sum(x => x.Recipe.Price);

            var recipes = shoppingCartItems.Select(x => new OrderRecipe { RecipeId = x.RecipeId }).ToList();
            orderForDb.OrderRecipes = recipes;

            await this.orderRepository.SaveChangesAsync();
            return true;
        }

        public OrderStatus GetOrderStatusByName(string orderStatusName)
        {
            var status = this.orderStatusRepository.All().SingleOrDefault(x => x.Name == orderStatusName);

            return status;
        }
    }
}

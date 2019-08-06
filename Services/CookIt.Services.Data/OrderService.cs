namespace CookIt.Services.Data
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using AutoMapper;
    using CookIt.Common;
    using CookIt.Data.Common.Repositories;
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> orderRepository;
        private readonly IRepository<OrderStatus> orderStatusRepository;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IRepository<OrderRecipe> orderRecipeRepository;

        public OrderService(
            IRepository<Order> orderRepository,
            IRepository<OrderStatus> orderStatusRepository,
            IShoppingCartService shoppingCartService,
            IRepository<OrderRecipe> orderRecipeRepository)
        {
            this.orderRepository = orderRepository;
            this.orderStatusRepository = orderStatusRepository;
            this.shoppingCartService = shoppingCartService;
            this.orderRecipeRepository = orderRecipeRepository;
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

        public async Task<bool> Checkout<TModel>(TModel order, string issuerId)
        {
            var orderForDb = Mapper.Map<Order>(order);

            var shoppingCartItems = await this.shoppingCartService.CheckOutGetCartItems(issuerId);
            var recipes = shoppingCartItems.Select(x => new OrderRecipe { RecipeId = x.RecipeId }).ToList();
            orderForDb.OrderRecipes = recipes;

            await this.orderRepository.AddAsync(orderForDb);
            await this.orderRepository.SaveChangesAsync();
            return true;
        }

        public IQueryable<TModel> GetAllOrders<TModel>()
        {
            var orders = this.orderRepository.All().To<TModel>();

            return orders;
        }

        public OrderStatus GetOrderStatusByName(string orderStatusName)
        {
            var status = this.orderStatusRepository.All().SingleOrDefault(x => x.Name == orderStatusName);

            return status;
        }

        public async Task<bool> ConfirmOrder(string orderId)
        {
            var order = this.orderRepository.All().SingleOrDefault(x => x.Id == orderId);
            var processedStatus = this.GetOrderStatusByName(GlobalConstants.ProcessedOrderStatus);
            order.OrderStatusId = processedStatus.Id;

            this.orderRepository.Update(order);
            await this.orderRepository.SaveChangesAsync();

            return true;
        }

        public bool IsPending(string orderId)
        {
            var pendingStatus = this.GetOrderStatusByName(GlobalConstants.PendingOrderStatus);
            var order = this.orderRepository.All().SingleOrDefault(x => x.Id == orderId);

            return order.OrderStatusId == pendingStatus.Id;
        }

        public async Task<bool> CancelOrder(string orderId)
        {
            var order = this.orderRepository.All().SingleOrDefault(x => x.Id == orderId);

            this.orderRepository.Delete(order);
            await this.orderRepository.SaveChangesAsync();
            return true;
        }

        public string GetIssuerEmailByOrderId(string orderId)
        {
            var email = this.orderRepository.All().Include(x => x.Issuer).SingleOrDefault(x => x.Id == orderId).Issuer.Email;

            return email;
        }

        public IQueryable<TModel> FindOrderById<TModel>(string orderId)
        {
            var order = this.orderRepository.All().Where(x => x.Id == orderId).To<TModel>();

            return order;
        }

        public IQueryable<TModel> GetRecipesFromOrder<TModel>(string orderId)
        {
            var recipes = this.orderRecipeRepository.All().Where(x => x.OrderId == orderId).To<TModel>();

            return recipes;
        }

        public bool HasOrderWithId(string userId, string orderId)
        {
            var order = this.orderRepository.All().SingleOrDefault(x => x.Id == orderId);

            return userId == order.IssuerId;
        }

        public IQueryable<TModel> GetAllOrdersByUserId<TModel>(string userId)
        {
            var orders = this.orderRepository.All().Where(x => x.IssuerId == userId).To<TModel>();

            return orders;
        }

        public bool HasOrderWithAddressId(int addressId)
        {
            return this.orderRepository.All().Any(x => x.AddressId == addressId);
        }

        public async Task<bool> TakeOrder(string orderId, string courierId)
        {
            var order = this.orderRepository.All().SingleOrDefault(x => x.Id == orderId);
            var gettingIngredientsStatus = this.orderStatusRepository.All().SingleOrDefault(x => x.Name == GlobalConstants.GettingIngredientsOrderStatus);

            order.CourierId = courierId;
            order.OrderStatusId = gettingIngredientsStatus.Id;

            this.orderRepository.Update(order);
            await this.orderRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeliverOrder(string orderId)
        {
            var order = this.orderRepository.All().SingleOrDefault(x => x.Id == orderId);
            var deliveringStatus = this.orderStatusRepository.All().SingleOrDefault(x => x.Name == GlobalConstants.DeliveringOrderStatus);

            order.OrderStatusId = deliveringStatus.Id;

            this.orderRepository.Update(order);
            await this.orderRepository.SaveChangesAsync();

            return true;
        }

        public bool IsAtStatus(string orderId, string statusName)
        {
            var order = this.orderRepository.All().Include(x => x.OrderStatus).SingleOrDefault(x => x.Id == orderId);

            return order.OrderStatus.Name == statusName;
        }

        public async Task<bool> AcquiredOrder(string orderId)
        {
            var order = this.orderRepository.All().SingleOrDefault(x => x.Id == orderId);
            var acquiredStatus = this.orderStatusRepository.All().SingleOrDefault(x => x.Name == GlobalConstants.AcquiredOrderStatus);

            order.OrderStatusId = acquiredStatus.Id;

            this.orderRepository.Update(order);
            await this.orderRepository.SaveChangesAsync();

            return true;
        }

        public string GetAllRecipesInstructionsForOrder(string id)
        {
            var order = this.orderRepository
                .All()
                .Include(x => x.OrderRecipes)
                .ThenInclude(c => c.Recipe)
                .SingleOrDefault(x => x.Id == id);

            StringBuilder sb = new StringBuilder();

            foreach (var recipe in order.OrderRecipes)
            {
                sb.AppendLine($"{recipe.Recipe.Name}:");
                sb.AppendLine($"{recipe.Recipe.RecipeInstructions}");
                sb.AppendLine(Environment.NewLine);
            }

            return sb.ToString().TrimEnd();
        }
    }
}

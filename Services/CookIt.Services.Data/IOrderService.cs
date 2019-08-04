namespace CookIt.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookIt.Data.Models;

    public interface IOrderService
    {
        Task<bool> Checkout<TModel>(TModel order, string issuerId);

        OrderStatus GetOrderStatusByName(string orderStatusName);

        IQueryable<TModel> GetAllProcessedOrders<TModel>();

        IQueryable<TModel> GetAllPendingOrders<TModel>();

        Task<bool> ConfirmOrder(string orderId);

        bool IsPending(string orderId);

        Task<bool> CancelOrder(string orderId);

        string GetIssuerEmailByOrderId(string orderId);

        IQueryable<TModel> FindOrderById<TModel>(string orderId);

        IQueryable<TModel> GetRecipesFromOrder<TModel>(string orderId);

        bool HasOrderWithId(string userId, string orderId);

        IQueryable<TModel> GetAllOrdersByUserId<TModel>(string userId);
    }
}

namespace CookIt.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookIt.Data.Models;

    public interface IOrderService
    {
        Task<bool> Checkout<TModel>(TModel order, string issuerId);

        OrderStatus GetOrderStatusByName(string orderStatusName);

        Task<bool> ConfirmOrder(string orderId);

        bool IsPending(string orderId);

        Task<bool> CancelOrder(string orderId);

        string GetIssuerEmailByOrderId(string orderId);

        IQueryable<TModel> FindOrderById<TModel>(string orderId);

        IQueryable<TModel> GetRecipesFromOrder<TModel>(string orderId);

        bool HasOrderWithId(string userId, string orderId);

        IQueryable<TModel> GetAllOrdersByUserId<TModel>(string userId);

        bool HasOrderWithAddressId(int addressId);

        IQueryable<TModel> GetAllOrders<TModel>();

        Task<bool> TakeOrder(string orderId, string courierId);

        Task<bool> DeliverOrder(string orderId);

        bool IsAtStatus(string orderId, string statusName);

        Task<bool> AcquiredOrder(string orderId);

        string GetAllRecipesInstructionsForOrder(string id);

    }
}

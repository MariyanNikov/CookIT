namespace CookIt.Services.Data
{
    using System.Threading.Tasks;

    using CookIt.Data.Models;

    public interface IOrderService
    {
        Task<bool> Checkout<TModel>(TModel order);

        OrderStatus GetOrderStatusByName(string orderStatusName);
    }
}

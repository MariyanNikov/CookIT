namespace CookIt.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IShoppingCartService
    {
        int GetShoppingCartItemsCount(string userId);

        Task<bool> CreateShoppingCart(string userId);

        Task<bool> AddShoppingCartItem(string userId, int recipeId);

        Task<bool> RemoveShoppingCartItem(string userId, int recipeId);

        Task<bool> ClearShoppingCart(string userId);

        IQueryable<TModel> GetAllShoppingCartItems<TModel>(string userid);
    }
}

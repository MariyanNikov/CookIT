namespace CookIt.Services.Data
{
    using CookIt.Data.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IShoppingCartService
    {
        int GetShoppingCartItemsCount(string userId);

        Task<bool> CreateShoppingCart(string userId);

        Task<bool> AddShoppingCartItem(string userId, int recipeId);

        Task<bool> RemoveShoppingCartItem(string userId, int recipeId);

        Task<bool> ClearShoppingCart(string userId);

        bool IsInShoppingCart(string userId, int recipeId);

        IQueryable<TModel> GetAllShoppingCartItems<TModel>(string userId);

        bool HasItemsInCart(string userId);

        Task<ICollection<ShoppingCartItem>> CheckOutGetCartItems(string userId);
    }
}

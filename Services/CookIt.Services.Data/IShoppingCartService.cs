namespace CookIt.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookIt.Data.Models;

    public interface IShoppingCartService
    {
        Task<int> GetShoppingCartItemsCount(string userId);

        Task<bool> CreateShoppingCart(string userId);

        decimal GetPriceOfAllShoppingCartItemsByUserId(string userId);

        Task<bool> AddShoppingCartItem(string userId, int recipeId);

        Task<bool> RemoveShoppingCartItem(string userId, int recipeId);

        Task<bool> ClearShoppingCart(string userId);

        bool IsInShoppingCart(string userId, int recipeId);

        IQueryable<TModel> GetAllShoppingCartItems<TModel>(string userId);

        bool HasItemsInCart(string userId);

        ICollection<ShoppingCartItem> CheckOutGetCartItems(string userId);
    }
}

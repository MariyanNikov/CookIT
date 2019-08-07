namespace CookIt.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookIt.Data.Common.Repositories;
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> shoppingCartRepository;
        private readonly IRepository<ShoppingCartItem> shoppingCartItemsRepository;

        public ShoppingCartService(
            IRepository<ShoppingCart> shoppingCartRepository,
            IRepository<ShoppingCartItem> shoppingCartItemsRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.shoppingCartItemsRepository = shoppingCartItemsRepository;
        }

        public async Task<bool> AddShoppingCartItem(string userId, int recipeId)
        {
            var cart = this.shoppingCartRepository.All().SingleOrDefault(x => x.ApplicationUserId == userId);
            await this.shoppingCartItemsRepository.AddAsync(new ShoppingCartItem { ShoppingCartId = cart.Id, RecipeId = recipeId });
            await this.shoppingCartItemsRepository.SaveChangesAsync();
            return true;
        }

        public async Task<ICollection<ShoppingCartItem>> CheckOutGetCartItems(string userId)
        {
            var cartId = this.shoppingCartRepository.All().SingleOrDefault(x => x.ApplicationUserId == userId).Id;

            var cartItems = await this.shoppingCartItemsRepository.All().Include(x => x.Recipe).Where(x => x.ShoppingCartId == cartId).ToListAsync();

            return cartItems;
        }

        public async Task<bool> ClearShoppingCart(string userId)
        {
            var cart = this.shoppingCartRepository.All().Include(x => x.CartItems).SingleOrDefault(x => x.ApplicationUserId == userId).CartItems;

            this.shoppingCartItemsRepository.DeleteAll(cart);
            await this.shoppingCartItemsRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CreateShoppingCart(string userId)
        {
            await this.shoppingCartRepository.AddAsync(new ShoppingCart { ApplicationUserId = userId });
            await this.shoppingCartRepository.SaveChangesAsync();
            return true;
        }

        public IQueryable<TModel> GetAllShoppingCartItems<TModel>(string userId)
        {
            var cartId = this.shoppingCartRepository.All().SingleOrDefault(x => x.ApplicationUserId == userId).Id;

            var cartItems = this.shoppingCartItemsRepository.All().Where(x => x.ShoppingCartId == cartId).To<TModel>();

            return cartItems;
        }

        public async Task<int> GetShoppingCartItemsCount(string userId)
        {
            var cart = this.shoppingCartRepository.All().SingleOrDefault(x => x.ApplicationUserId == userId);
            if (cart == null)
            {
                await this.CreateShoppingCart(userId);
            }

            var cartItemsCount = this.shoppingCartRepository.All().Include(x => x.CartItems).SingleOrDefault(x => x.ApplicationUserId == userId).CartItems.Count();

            return cartItemsCount;
        }

        public bool HasItemsInCart(string userId)
        {
            bool hasItems = this.shoppingCartRepository.All().Include(x => x.CartItems).SingleOrDefault(x => x.ApplicationUserId == userId).CartItems.Any();

            return hasItems;
        }

        public bool IsInShoppingCart(string userId, int recipeId)
        {
            var cart = this.shoppingCartRepository.All().Include(x => x.CartItems).SingleOrDefault(x => x.ApplicationUserId == userId);

            return cart.CartItems.Any(x => x.RecipeId == recipeId);
        }

        public async Task<bool> RemoveShoppingCartItem(string userId, int recipeId)
        {
            var cart = this.shoppingCartRepository.All().Include(x => x.CartItems).SingleOrDefault(x => x.ApplicationUserId == userId);
            var cartItem = cart.CartItems.FirstOrDefault(x => x.RecipeId == recipeId);
            if (cartItem != null)
            {
                this.shoppingCartItemsRepository.Delete(cartItem);
                await this.shoppingCartItemsRepository.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}

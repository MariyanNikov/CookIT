namespace CookIt.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookIt.Data.Common.Repositories;
    using CookIt.Data.Models;
    using CookIt.Web.BindingModels.Recipe;
    using CookIt.Web.ViewModels.Recipe;
    using CookIt.Web.ViewModels.Review;
    using CookIt.Web.ViewModels.ShoppingCart;
    using Moq;

    using Xunit;

    public class ShoppingCartServiceTests
    {
        public ShoppingCartServiceTests()
        {
            MapperInitialize.Initialize();
        }

        [Fact]
        public async Task AddShoppingCartItemShouldWorkProperly()
        {
            var shoppingCartRepository = new Mock<IRepository<ShoppingCart>>();
            var shoppingCartItemRepository = new Mock<IRepository<ShoppingCartItem>>();
            shoppingCartRepository.Setup(x => x.All())
                .Returns(new List<ShoppingCart> { new ShoppingCart { ApplicationUserId = "1" } }.AsQueryable());
            var service = new ShoppingCartService(shoppingCartRepository.Object, shoppingCartItemRepository.Object);

            var actualResult = await service.AddShoppingCartItem("1", 1);

            Assert.True(actualResult == true, "Add ShoppingCartItem should return true upon successfully adding shopping cart item.");
            shoppingCartRepository.Verify(x => x.All(), Times.Once, "All method of ShoppingCartRepository should be called once.");
            shoppingCartItemRepository.Verify(x => x.AddAsync(It.IsAny<ShoppingCartItem>()), Times.Once, "AddAsync method of ShoppingCartItemRepository should be called once.");
            shoppingCartItemRepository.Verify(x => x.SaveChangesAsync(), Times.Once, "SaveChangesAsync method of ShoppingCartItemRepository should be called once.");
        }

        [Fact]
        public void GetPriceOfAllShoppingCartItemsByUserIdShouldReturnCorrectSum()
        {
            var shoppingCartRepository = new Mock<IRepository<ShoppingCart>>();
            var shoppingCartItemRepository = new Mock<IRepository<ShoppingCartItem>>();
            shoppingCartRepository.Setup(x => x.All()).Returns(this.DummyDataShoppingCarts().AsQueryable());
            shoppingCartItemRepository.Setup(x => x.All()).Returns(this.DummyDataShoppingCartItems().AsQueryable());

            var service = new ShoppingCartService(shoppingCartRepository.Object, shoppingCartItemRepository.Object);

            var actualResult = service.GetPriceOfAllShoppingCartItemsByUserId("1");
            var expectedResult = this.DummyDataShoppingCartItems().Where(x => x.ShoppingCartId == 1).Sum(x => x.Recipe.Price);

            Assert.True(actualResult == expectedResult, "Total Price does not match.");
        }

        [Fact]
        public void CheckOutGetCartItemsShouldWorkProperly()
        {
            var shoppingCartRepository = new Mock<IRepository<ShoppingCart>>();
            var shoppingCartItemRepository = new Mock<IRepository<ShoppingCartItem>>();
            shoppingCartRepository.Setup(x => x.All()).Returns(this.DummyDataShoppingCarts().AsQueryable());
            shoppingCartItemRepository.Setup(x => x.All()).Returns(this.DummyDataShoppingCartItems().AsQueryable());

            var service = new ShoppingCartService(shoppingCartRepository.Object, shoppingCartItemRepository.Object);

            var actualResult = service.CheckOutGetCartItems("1");
            var expectedResult = this.DummyDataShoppingCartItems().Where(x => x.ShoppingCartId == 1).ToList();

            Assert.True(actualResult.Count == expectedResult.Count);
        }

        [Fact]
        public async Task ClearShoppingCartShouldWorkProperly()
        {
            var shoppingCartRepository = new Mock<IRepository<ShoppingCart>>();
            var shoppingCartItemRepository = new Mock<IRepository<ShoppingCartItem>>();
            shoppingCartRepository.Setup(x => x.All()).Returns(this.DummyDataShoppingCarts().AsQueryable());
            shoppingCartItemRepository.Setup(x => x.All()).Returns(this.DummyDataShoppingCartItems().AsQueryable());

            var service = new ShoppingCartService(shoppingCartRepository.Object, shoppingCartItemRepository.Object);

            var actualResult = await service.ClearShoppingCart("1");

            Assert.True(actualResult == true, "ClearShoppingCart should return true upon clearing the cart successfully");
            shoppingCartRepository.Verify(x => x.All(), Times.Once, "All Method of shopping cart repository should be called once.");
            shoppingCartItemRepository.Verify(x => x.DeleteAll(It.IsAny<ICollection<ShoppingCartItem>>()), Times.Once, "DeleteAll method of shopping cart items repository should be called once");
            shoppingCartItemRepository.Verify(x => x.SaveChangesAsync(), Times.Once, "SaveChangesAsync of shopping cart items repository should be called once.");
        }

        [Fact]
        public async Task CreateShoppingCartShouldWorkProperly()
        {
            var shoppingCartRepository = new Mock<IRepository<ShoppingCart>>();
            var shoppingCartItemRepository = new Mock<IRepository<ShoppingCartItem>>();
            var service = new ShoppingCartService(shoppingCartRepository.Object, shoppingCartItemRepository.Object);

            var actualResult = await service.CreateShoppingCart("1");

            Assert.True(actualResult == true, "Create Shopping Cart should return true upon successfully creating cart.");
            shoppingCartRepository.Verify(x => x.AddAsync(It.IsAny<ShoppingCart>()), Times.Once, "AddAsync method of Shopping Cart Repository should be called once.");
            shoppingCartRepository.Verify(x => x.SaveChangesAsync(), Times.Once, "SaveChangesAsync method of Shopping Cart Repository should be called once.");
        }

        [Fact]
        public void GetAllShoppingCartItemsShouldReturnProperData()
        {
            var shoppingCartRepository = new Mock<IRepository<ShoppingCart>>();
            var shoppingCartItemRepository = new Mock<IRepository<ShoppingCartItem>>();
            shoppingCartRepository.Setup(x => x.All()).Returns(this.DummyDataShoppingCarts().AsQueryable());
            shoppingCartItemRepository.Setup(x => x.All()).Returns(this.DummyDataShoppingCartItems().AsQueryable());

            var service = new ShoppingCartService(shoppingCartRepository.Object, shoppingCartItemRepository.Object);

            var actualResults = service.GetAllShoppingCartItems<CartItemsViewModel>("1").ToList();
            var expectedResults = this.DummyDataShoppingCartItems();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry.Recipe.Id == actualEntry.Id, "Shopping Cart Item - Recipe Id does not match.");
                Assert.True(expectedEntry.Recipe.Name == actualEntry.Name, "Shopping Cart Item - Recipe Name does not match");
                Assert.True(expectedEntry.Recipe.Image == actualEntry.Image, "Shopping Cart Item - Recipe Image does not match");
                Assert.True(expectedEntry.Recipe.Portions == actualEntry.Portions, "Shopping Cart Item - Recipe Portions does not match");
                Assert.True(expectedEntry.Recipe.Price == actualEntry.Price, "Shopping Cart Item - Recipe Price does not match");
                Assert.True(expectedEntry.Recipe.RecipeIngredients.Count == actualEntry.ListOfIngredients.Split(", ").ToList().Count, "Shopping Cart Item - Recipe Ingredients Count does not match.");
            }
        }

        [Fact]
        public async Task GetShoppingCartItemsCountShouldWorkProperly()
        {
            var shoppingCartRepository = new Mock<IRepository<ShoppingCart>>();
            var shoppingCartItemRepository = new Mock<IRepository<ShoppingCartItem>>();
            shoppingCartRepository.Setup(x => x.All()).Returns(this.DummyDataShoppingCarts().AsQueryable());
            shoppingCartItemRepository.Setup(x => x.All()).Returns(this.DummyDataShoppingCartItems().AsQueryable());

            var service = new ShoppingCartService(shoppingCartRepository.Object, shoppingCartItemRepository.Object);

            var actualResult = await service.GetShoppingCartItemsCount("1");
            var expectedResult = this.DummyDataShoppingCarts().SingleOrDefault(x => x.ApplicationUserId == "1").CartItems.Count;

            Assert.True(actualResult == expectedResult, $"Get Shopping Cart Items Count does not match.Expected: {expectedResult}");
        }

        [Theory]
        [InlineData("1", true)]
        [InlineData("2", false)]
        public void HasItemsInCartShouldWorkProperly(string userId, bool expectedResult)
        {
            var shoppingCartRepository = new Mock<IRepository<ShoppingCart>>();
            var shoppingCartItemRepository = new Mock<IRepository<ShoppingCartItem>>();
            shoppingCartRepository.Setup(x => x.All()).Returns(this.DummyDataShoppingCarts().AsQueryable());

            var service = new ShoppingCartService(shoppingCartRepository.Object, shoppingCartItemRepository.Object);

            var actualResult = service.HasItemsInCart(userId);

            Assert.True(actualResult == expectedResult, $"Has Items IN cart should return {expectedResult} with userId: {userId}");
        }

        [Theory]
        [InlineData("1", 1, true)]
        [InlineData("1", 4, false)]
        public void IsInShoppingCartShouldWorkProperly(string userId, int recipeId, bool expectedResult)
        {
            var shoppingCartRepository = new Mock<IRepository<ShoppingCart>>();
            var shoppingCartItemRepository = new Mock<IRepository<ShoppingCartItem>>();
            shoppingCartRepository.Setup(x => x.All()).Returns(this.DummyDataShoppingCarts().AsQueryable());

            var service = new ShoppingCartService(shoppingCartRepository.Object, shoppingCartItemRepository.Object);

            var actualResult = service.IsInShoppingCart(userId, recipeId);

            Assert.True(actualResult == expectedResult, $"Is In Shopping Cart should return {expectedResult} with userId: {userId} and recipeId: {recipeId}");
        }

        [Theory]
        [InlineData("1", 1, true)]
        [InlineData("1", 4, false)]
        public async Task RemoveShoppingCartItemShouldRemoveItemProperly(string userId, int recipeId, bool expectedResult)
        {
            var shoppingCartRepository = new Mock<IRepository<ShoppingCart>>();
            var shoppingCartItemRepository = new Mock<IRepository<ShoppingCartItem>>();
            shoppingCartRepository.Setup(x => x.All()).Returns(this.DummyDataShoppingCarts().AsQueryable());

            var service = new ShoppingCartService(shoppingCartRepository.Object, shoppingCartItemRepository.Object);

            var actualResult = await service.RemoveShoppingCartItem(userId, recipeId);

            Assert.True(actualResult == expectedResult, $"Remove Shopping Cart Item should return {expectedResult} with userId: {userId} and recipeId: {recipeId}");
            if (expectedResult == true)
            {
                shoppingCartItemRepository.Verify(x => x.Delete(It.IsAny<ShoppingCartItem>()), Times.Once, "Delete method of Shopping Cart Item Repository should be called once.");
                shoppingCartItemRepository.Verify(x => x.SaveChangesAsync(), Times.Once, "SaveChangesAsync method of Shopping Cart Item Repository should be called once.");
            }
        }

        private List<ShoppingCart> DummyDataShoppingCarts()
        {
            var cart = new ShoppingCart
            {
                ApplicationUserId = "1",
                Id = 1,
                CartItems = new List<ShoppingCartItem>
            {
                new ShoppingCartItem
                {
                    Id = 1,
                    ShoppingCartId = 1,
                    Recipe = new Recipe
                    {
                        Id = 1,
                        Price = 3.99M,
                    },
                    RecipeId = 1,
                },
                new ShoppingCartItem
                {
                    Id = 2,
                    ShoppingCartId = 1,
                    Recipe = new Recipe
                    {
                        Id = 2,
                        Price = 8.99M,
                    },
                    RecipeId = 2,
                },
                new ShoppingCartItem
                {
                    Id = 3,
                    ShoppingCartId = 1,
                    Recipe = new Recipe
                    {
                        Id = 3,
                        Price = 5.99M,
                    },
                    RecipeId = 3,
                },
            },
            };

            return new List<ShoppingCart> { cart, new ShoppingCart { ApplicationUserId = "2", Id = 2 } };
        }

        private List<ShoppingCartItem> DummyDataShoppingCartItems()
        {
            var items = new List<ShoppingCartItem>
            {
                new ShoppingCartItem
                {
                    ShoppingCartId = 1,
                    Recipe = new Recipe
                    {
                        Id = 1,
                        Image = "Link1",
                        Name = "Musaka1",
                        Portions = 1,
                        Price = 3.99M,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient
                            {
                                Ingredient = new Ingredient
                                {
                                    Name = "Beef",
                                },
                            },
                            new RecipeIngredient
                            {
                                Ingredient = new Ingredient
                                {
                                    Name = "Cucumber",
                                },
                            },
                            new RecipeIngredient
                            {
                                Ingredient = new Ingredient
                                {
                                    Name = "Tomatos",
                                },
                            },
                        },
                    },
                },
                new ShoppingCartItem
                {
                    ShoppingCartId = 1,
                    Recipe = new Recipe
                    {
                        Id = 2,
                        Image = "Link2",
                        Name = "Musaka2",
                        Portions = 2,
                        Price = 8.99M,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient
                            {
                                Ingredient = new Ingredient
                                {
                                    Name = "Beef2",
                                },
                            },
                            new RecipeIngredient
                            {
                                Ingredient = new Ingredient
                                {
                                    Name = "Cucumber2",
                                },
                            },
                            new RecipeIngredient
                            {
                                Ingredient = new Ingredient
                                {
                                    Name = "Tomatos2",
                                },
                            },
                        },
                    },
                },
                new ShoppingCartItem
                {
                    ShoppingCartId = 1,
                    Recipe = new Recipe
                    {
                        Id = 3,
                        Image = "Link3",
                        Name = "Musaka3",
                        Portions = 3,
                        Price = 2.99M,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
                            new RecipeIngredient
                            {
                                Ingredient = new Ingredient
                                {
                                    Name = "Beef3",
                                },
                            },
                            new RecipeIngredient
                            {
                                Ingredient = new Ingredient
                                {
                                    Name = "Cucumber3",
                                },
                            },
                            new RecipeIngredient
                            {
                                Ingredient = new Ingredient
                                {
                                    Name = "Tomatos3",
                                },
                            },
                        },
                    },
                },
            };
            return items;
        }
    }
}

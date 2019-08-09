namespace CookIt.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using CookIt.Common;
    using CookIt.Data.Common.Repositories;
    using CookIt.Data.Models;
    using CookIt.Web.BindingModels.Order;
    using CookIt.Web.BindingModels.Recipe;
    using CookIt.Web.ViewModels.Order;
    using CookIt.Web.ViewModels.Recipe;
    using Moq;

    using Xunit;

    public class OrderServiceTests
    {
        public OrderServiceTests()
        {
            MapperInitialize.Initialize();
        }

        [Fact]
        public async Task CheckoutShouldWorkProperly()
        {
            var orderRepository = new Mock<IRepository<Order>>();
            var orderStatusRepository = new Mock<IRepository<OrderStatus>>();
            var orderRecipeRepository = new Mock<IRepository<OrderRecipe>>();

            var shoppingCartRepository = new Mock<IRepository<ShoppingCart>>();
            shoppingCartRepository.Setup(x => x.All()).Returns(this.DummyDataShoppingCarts().AsQueryable());
            var shoppingCartItemsRepository = new Mock<IRepository<ShoppingCartItem>>();
            shoppingCartItemsRepository.Setup(x => x.All()).Returns(this.DummyDataShoppingCartItems().AsQueryable());

            var cartService = new ShoppingCartService(shoppingCartRepository.Object, shoppingCartItemsRepository.Object);
            var orderService = new OrderService(orderRepository.Object, orderStatusRepository.Object, cartService, orderRecipeRepository.Object);

            var actualResult = await orderService.Checkout<CheckoutBindingModel>(new CheckoutBindingModel(), "1");

            Assert.True(actualResult == true, "Checkout should return true upon successfully creating order.");
            shoppingCartRepository.Verify(x => x.All(), Times.Once, "All method of shopping cart repository should be called once.");
            shoppingCartItemsRepository.Verify(x => x.All(), Times.Once, "All method of shopping cart items repository should be called once.");
            orderRepository.Verify(x => x.AddAsync(It.IsAny<Order>()), Times.Once, "AddAsync method of Order Repository should be called once.");
            orderRepository.Verify(x => x.SaveChangesAsync(), Times.Once, "SaveChangesAsync method of Order Repository should be called once.");
        }

        [Fact]
        public void GetAllOrdersShouldWorkProperly()
        {
            var orderRepository = new Mock<IRepository<Order>>();

            orderRepository.Setup(x => x.All()).Returns(this.DummyDataOrders().AsQueryable());
            var orderService = new OrderService(orderRepository.Object, null, null, null);

            var actualResults = orderService.GetAllOrders<OrderAllViewModel>().ToList();
            var expectedResults = this.DummyDataOrders();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(actualEntry.Id == expectedEntry.Id, "Order Id does not match.");
                Assert.True(actualEntry.IssuedOn.Day == expectedEntry.IssuedOn.Day, "Order Issued On does not match.");
                Assert.True(actualEntry.DeliveryDate.Day == expectedEntry.DeliveryDate.Day, "Order Delivery Date does not match.");
                Assert.True(actualEntry.OrderStatusName == expectedEntry.OrderStatus.Name, "Order Status Name does not match.");
                Assert.True(actualEntry.FullNameIssuer == expectedEntry.FullNameIssuer, "Order FullNameIssuer does not match.");
                Assert.True(actualEntry.Address == string.Join(" - ", expectedEntry.Address.StreetAddress, expectedEntry.Address.City, expectedEntry.Address.CityCode), "Order Address does not match.");
                Assert.True(actualEntry.CourierFullName == expectedEntry.Courier.FirstName + " " + expectedEntry.Courier.LastName, "Order CourierFullName does not match.");
            }
        }

        [Theory]
        [InlineData(GlobalConstants.PendingOrderStatus, 1)]
        [InlineData(GlobalConstants.ProcessedOrderStatus, 2)]
        [InlineData(GlobalConstants.GettingIngredientsOrderStatus, 3)]
        [InlineData(GlobalConstants.DeliveringOrderStatus, 4)]
        [InlineData(GlobalConstants.AcquiredOrderStatus, 5)]
        public void GetOrderStatusIdByNameShouldReturnCorrectId(string statusName, int expectedResult)
        {
            var orderStatusRepository = new Mock<IRepository<OrderStatus>>();
            orderStatusRepository.Setup(x => x.All()).Returns(this.DummyDataOrderStatuses().AsQueryable());

            var orderService = new OrderService(null, orderStatusRepository.Object, null, null);

            var actualResult = orderService.GetOrderStatusIdByName(statusName);

            Assert.True(actualResult == expectedResult, $"GetOrderStatusIdByName should return {expectedResult} for statusName: {statusName}");
        }

        [Theory]
        [InlineData(GlobalConstants.PendingOrderStatus, "Pending")]
        [InlineData("status", null)]
        public void GetOrderStatusByNameShouldReturnCorrectStatus(string statusName, string expectedStatusName)
        {
            var orderStatusRepository = new Mock<IRepository<OrderStatus>>();
            orderStatusRepository.Setup(x => x.All()).Returns(this.DummyDataOrderStatuses().AsQueryable());

            var orderService = new OrderService(null, orderStatusRepository.Object, null, null);

            var actualResult = orderService.GetOrderStatusByName(statusName);

            Assert.True(actualResult?.Name == expectedStatusName, $"GetOrderStatusByName should return {expectedStatusName} with Status Name: {statusName}");
        }

        [Fact]
        public async Task ConfirmOrderShouldProperlySetStatusToProcessed()
        {
            var orderRepository = new Mock<IRepository<Order>>();
            var orderStatusRepository = new Mock<IRepository<OrderStatus>>();
            orderRepository.Setup(x => x.All()).Returns(this.DummyDataOrders().AsQueryable());
            orderStatusRepository.Setup(x => x.All()).Returns(this.DummyDataOrderStatuses().AsQueryable());

            var orderService = new OrderService(orderRepository.Object, orderStatusRepository.Object, null, null);

            var actualResult = await orderService.ConfirmOrder("1");

            Assert.True(actualResult == true, "Confirm Order should return true upon successfully changin the status to Processed.");
            orderStatusRepository.Verify(x => x.All(), Times.Once, "All method of Order Status Repository should be called once.");
            orderRepository.Verify(x => x.Update(It.IsAny<Order>()), Times.Once, "Update method of Order Repository should be called once.");
            orderRepository.Verify(x => x.SaveChangesAsync(), Times.Once, "SaveChangesAsync method of Order Repository should be called once.");
        }

        [Theory]
        [InlineData("1", true)]
        [InlineData("2", false)]
        public void IsPendingShouldReturnCorrectlyIfOrderIsInPendingStatus(string orderId, bool expectedResult)
        {
            var orderRepository = new Mock<IRepository<Order>>();
            var orderStatusRepository = new Mock<IRepository<OrderStatus>>();
            orderRepository.Setup(x => x.All()).Returns(this.DummyDataOrders().AsQueryable());
            orderStatusRepository.Setup(x => x.All()).Returns(this.DummyDataOrderStatuses().AsQueryable());

            var orderService = new OrderService(orderRepository.Object, orderStatusRepository.Object, null, null);

            var actualResult = orderService.IsPending(orderId);

            Assert.True(actualResult == expectedResult, $"IsPending should return {expectedResult} with orderId: {orderId}");
        }

        [Fact]
        public async Task CancelOrderShouldProperlyDeleteOrder()
        {
            var orderRepository = new Mock<IRepository<Order>>();
            orderRepository.Setup(x => x.All()).Returns(this.DummyDataOrders().AsQueryable());

            var orderService = new OrderService(orderRepository.Object, null, null, null);

            var actualResult = await orderService.CancelOrder("1");

            Assert.True(actualResult == true, "CancelOrder should return true upon successfully removing order.");
            orderRepository.Verify(x => x.Delete(It.IsAny<Order>()), Times.Once, "Delete method of Order Repository should be called once.");
            orderRepository.Verify(x => x.SaveChangesAsync(), Times.Once, "SaveChangesAsync method of Order Repository should be called once.");
        }

        [Fact]
        public void GetIssuerEmailByOrderIdShouldReturnCorrectEmail()
        {
            var orderRepository = new Mock<IRepository<Order>>();
            orderRepository.Setup(x => x.All()).Returns(this.DummyDataOrders().AsQueryable());

            var orderService = new OrderService(orderRepository.Object, null, null, null);

            var actualResult = orderService.GetIssuerEmailByOrderId("1");
            var expectedResult = this.DummyDataOrders().SingleOrDefault(x => x.Id == "1").Issuer.Email;
            Assert.True(actualResult == expectedResult, "Email does not match.");
        }

        [Fact]
        public void FindOrderByIdShouldReturnCorrectOrder()
        {
            var orderRepository = new Mock<IRepository<Order>>();
            orderRepository.Setup(x => x.All()).Returns(this.DummyDataOrders().AsQueryable());

            var orderService = new OrderService(orderRepository.Object, null, null, null);

            var actualResult = orderService.FindOrderById<OrderDetailsViewModel>("1").SingleOrDefault();
            var expectedResult = this.DummyDataOrders().SingleOrDefault(x => x.Id == "1");

            Assert.True(actualResult.Id == expectedResult.Id, "Order Id does not match.");
            Assert.True(actualResult.IssuedOn.Day == expectedResult.IssuedOn.Day, "Order IssuedOn does not match.");
            Assert.True(actualResult.DeliveryDate.Day == expectedResult.DeliveryDate.Day, "Order DeliveryDate does not match.");
            Assert.True(actualResult.FullNameIssuer == expectedResult.FullNameIssuer, "Order FullNameIssuer does not match.");
            Assert.True(actualResult.PhoneNumber == expectedResult.PhoneNumber, "Order PhoneNumber does not match.");
            Assert.True(actualResult.CommentIssuer == expectedResult.CommentIssuer, "Order CommentIssuer does not match.");
            Assert.True(actualResult.TotalPrice == expectedResult.TotalPrice, "Order TotalPrice does not match.");
            Assert.True(actualResult.DeliveryAddress == string.Join(" - ", expectedResult.Address.StreetAddress, expectedResult.Address.City, expectedResult.Address.CityCode), "Order DeliveryAddress does not match.");
            Assert.True(actualResult.OrderStatusName == expectedResult.OrderStatus.Name, "Order OrderStatusName does not match.");
            Assert.True(actualResult.CourierName == expectedResult.Courier.FirstName + " " + expectedResult.Courier.LastName, "Order CourierName does not match.");
        }

        [Fact]
        public void GetRecipesFromOrderShouldWorkProperly()
        {
            var orderRecipeRepository = new Mock<IRepository<OrderRecipe>>();
            orderRecipeRepository.Setup(x => x.All()).Returns(this.DummyDataOrderRecipe().AsQueryable());

            var orderService = new OrderService(null, null, null, orderRecipeRepository.Object);

            var actualResults = orderService.GetRecipesFromOrder<OrderDetailsRecipesViewModel>("1").ToList();
            var expectedResults = this.DummyDataOrderRecipe().Where(x => x.OrderId == "1").Select(x => x.Recipe).ToList();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(actualEntry.Id == expectedEntry.Id, "Order Recipe Id does not match.");
                Assert.True(actualEntry.Name == expectedEntry.Name, "Order Recipe Name does not match.");
                Assert.True(actualEntry.Image == expectedEntry.Image, "Order Recipe Image does not match.");
                Assert.True(actualEntry.Price == expectedEntry.Price, "Order Recipe Price does not match.");
                Assert.True(actualEntry.Portions == expectedEntry.Portions, "Order Recipe Portions does not match.");
                Assert.True(actualEntry.ListOfIngredients == string.Join(", ", expectedEntry.RecipeIngredients.Select(x => x.Ingredient.Name)), "Order Recipe ListOfIngredients does not match.");
            }
        }

        [Theory]
        [InlineData("1", "1", true)]
        [InlineData("1", "2", false)]
        public void HasOrderWithIdShouldWorkProperly(string orderId, string userId, bool expectedResult)
        {
            var orderRepository = new Mock<IRepository<Order>>();
            orderRepository.Setup(x => x.All()).Returns(this.DummyDataOrders().AsQueryable());

            var orderService = new OrderService(orderRepository.Object, null, null, null);

            var actualResult = orderService.HasOrderWithId(userId, orderId);

            Assert.True(actualResult == expectedResult, $"Has Order With Id should return {expectedResult} with UserId: {userId} and orderId: {orderId}");
        }

        [Fact]
        public void GetAllOrdersByUserIdShouldReturnCorrectData()
        {
            var orderRepository = new Mock<IRepository<Order>>();
            orderRepository.Setup(x => x.All()).Returns(this.DummyDataOrders().AsQueryable());

            var orderService = new OrderService(orderRepository.Object, null, null, null);

            var actualResults = orderService.GetAllOrdersByUserId<MyOrdersViewModel>("1").ToList();
            var expectedResults = this.DummyDataOrders().Where(x => x.IssuerId == "1").ToList();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(actualEntry.Id == expectedEntry.Id, "Order Id does not match.");
                Assert.True(actualEntry.OrderStatusName == expectedEntry.OrderStatus.Name, "Order Status Name does not match.");
                Assert.True(actualEntry.TotalPrice == expectedEntry.TotalPrice, "Order TotalPrice does not match.");
                Assert.True(actualEntry.Courier == expectedEntry.Courier.FirstName + " " + expectedEntry.Courier.LastName, "Order Courier does not match.");
                Assert.True(actualEntry.DeliveryAddress == string.Join(" - ", expectedEntry.Address.StreetAddress, expectedEntry.Address.City, expectedEntry.Address.CityCode), "Order Address does not match.");
                Assert.True(actualEntry.DeliveryDate.Day == expectedEntry.DeliveryDate.Day, "Order Delivery Date does not match.");
            }
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, false)]
        public void HasOrderWithAddressIdShouldWorkProperly(int addressId, bool expectedResult)
        {
            var orderRepository = new Mock<IRepository<Order>>();
            orderRepository.Setup(x => x.All()).Returns(this.DummyDataOrders().AsQueryable());

            var orderService = new OrderService(orderRepository.Object, null, null, null);

            var actualResult = orderService.HasOrderWithAddressId(addressId);

            Assert.True(actualResult == expectedResult, $"Has Order With AddressId should return {expectedResult} with addressId: {addressId}");
        }

        [Fact]
        public async Task TakeOrderShouldCorrectlyAssignCourierAndChangeStatus()
        {
            var orderRepository = new Mock<IRepository<Order>>();
            orderRepository.Setup(x => x.All()).Returns(this.DummyDataOrders().AsQueryable());

            var orderStatusRepository = new Mock<IRepository<OrderStatus>>();
            orderStatusRepository.Setup(x => x.All()).Returns(this.DummyDataOrderStatuses().AsQueryable());

            var orderService = new OrderService(orderRepository.Object, orderStatusRepository.Object, null, null);

            var actualResult = await orderService.TakeOrder("1", "1");

            Assert.True(actualResult == true, "TakeOrder should return true upon successfully assigning order to courier and changing the status.");
            orderRepository.Verify(x => x.All(), Times.Once, "All method of Order Repository should be called once.");
            orderStatusRepository.Verify(x => x.All(), Times.Once, "All method of Order Status Repository should be called once.");
            orderRepository.Verify(x => x.Update(It.IsAny<Order>()), Times.Once, "Update method of Order Repository should be called once.");
            orderRepository.Verify(x => x.SaveChangesAsync(), Times.Once, "SaveChangesAsync method of Order Repository should be called once.");
        }

        [Fact]
        public async Task DeliverOrderShouldWorkProperly()
        {
            var orderRepository = new Mock<IRepository<Order>>();
            orderRepository.Setup(x => x.All()).Returns(this.DummyDataOrders().AsQueryable());

            var orderStatusRepository = new Mock<IRepository<OrderStatus>>();
            orderStatusRepository.Setup(x => x.All()).Returns(this.DummyDataOrderStatuses().AsQueryable());

            var orderService = new OrderService(orderRepository.Object, orderStatusRepository.Object, null, null);

            var actualResult = await orderService.DeliverOrder("1");

            Assert.True(actualResult == true, "DeliverOrder should return true upon successfully changing the status of the order.");
            orderRepository.Verify(x => x.All(), Times.Once, "All method of Order Repository should be called once.");
            orderStatusRepository.Verify(x => x.All(), Times.Once, "All method of Order Status Repository should be called once.");
            orderRepository.Verify(x => x.Update(It.IsAny<Order>()), Times.Once, "Update method of Order Repository should be called once.");
            orderRepository.Verify(x => x.SaveChangesAsync(), Times.Once, "SaveChangesAsync method of Order Repository should be called once.");
        }

        [Fact]
        public async Task AcquireOrderShouldWorkProperly()
        {
            var orderRepository = new Mock<IRepository<Order>>();
            orderRepository.Setup(x => x.All()).Returns(this.DummyDataOrders().AsQueryable());

            var orderStatusRepository = new Mock<IRepository<OrderStatus>>();
            orderStatusRepository.Setup(x => x.All()).Returns(this.DummyDataOrderStatuses().AsQueryable());

            var orderService = new OrderService(orderRepository.Object, orderStatusRepository.Object, null, null);

            var actualResult = await orderService.AcquiredOrder("1");

            Assert.True(actualResult == true, "AcquiredOrder should return true upon successfully changing the status of the order.");
            orderRepository.Verify(x => x.All(), Times.Once, "All method of Order Repository should be called once.");
            orderStatusRepository.Verify(x => x.All(), Times.Once, "All method of Order Status Repository should be called once.");
            orderRepository.Verify(x => x.Update(It.IsAny<Order>()), Times.Once, "Update method of Order Repository should be called once.");
            orderRepository.Verify(x => x.SaveChangesAsync(), Times.Once, "SaveChangesAsync method of Order Repository should be called once.");
        }

        [Theory]
        [InlineData("1", GlobalConstants.PendingOrderStatus, true)]
        [InlineData("2", GlobalConstants.DeliveringOrderStatus, true)]
        [InlineData("2", GlobalConstants.PendingOrderStatus, false)]
        public void IsAtStatusShouldWorkProperly(string orderId, string statusName, bool expectedResult)
        {
            var orderRepository = new Mock<IRepository<Order>>();
            orderRepository.Setup(x => x.All()).Returns(this.DummyDataOrders().AsQueryable());

            var orderService = new OrderService(orderRepository.Object, null, null, null);

            var actualResult = orderService.IsAtStatus(orderId, statusName);

            Assert.True(actualResult == expectedResult, $"IsAtStatus should return {expectedResult} with orderId: {orderId} and statusName: {statusName}");
        }

        [Fact]
        public void GetAllRecipesInstructionsForOrderShouldReturnProperInstructions()
        {
            var orderRepository = new Mock<IRepository<Order>>();
            orderRepository.Setup(x => x.All()).Returns(this.DummyDataOrders().AsQueryable());

            var orderService = new OrderService(orderRepository.Object, null, null, null);

            var actualResult = orderService.GetAllRecipesInstructionsForOrder("3");
            var expectedSb = new StringBuilder();
            var expectedData = this.DummyDataOrders().SingleOrDefault(x => x.Id == "3");
            foreach (var recipe in expectedData.OrderRecipes)
            {
                expectedSb.AppendLine($"{recipe.Recipe.Name}:");
                expectedSb.AppendLine(recipe.Recipe.RecipeInstructions);
                expectedSb.AppendLine(Environment.NewLine);
            }

            var expectedResult = expectedSb.ToString().TrimEnd();

            Assert.True(actualResult == expectedResult);
        }

        private List<OrderRecipe> DummyDataOrderRecipe()
        {
            var orderRecipes = new List<OrderRecipe>
            {
                new OrderRecipe
                {
                    OrderId = "1",
                    Recipe = new Recipe
                    {
                        Id = 1,
                        Name = "Musaka",
                        Image = "ImageLink",
                        Price = 4.99M,
                        Portions = 1,
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
                                    Name = "Potatos",
                                },
                            },
                        },
                    },
                },
                new OrderRecipe
                {
                    OrderId = "1",
                    Recipe = new Recipe
                    {
                        Id = 1,
                        Name = "Tarator",
                        Image = "ImageLinkTarator",
                        Price = 2.99M,
                        Portions = 4,
                        RecipeIngredients = new List<RecipeIngredient>
                        {
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
                                    Name = "Dill",
                                },
                            },
                        },
                    },
                },
            };

            return orderRecipes;
        }

        private List<OrderStatus> DummyDataOrderStatuses()
        {
            var orderStatuses = new List<OrderStatus>
            {
                new OrderStatus
                {
                    Id = 1,
                    Name = GlobalConstants.PendingOrderStatus,
                },
                new OrderStatus
                {
                    Id = 2,
                    Name = GlobalConstants.ProcessedOrderStatus,
                },
                new OrderStatus
                {
                    Id = 3,
                    Name = GlobalConstants.GettingIngredientsOrderStatus,
                },
                new OrderStatus
                {
                    Id = 4,
                    Name = GlobalConstants.DeliveringOrderStatus,
                },
                new OrderStatus
                {
                    Id = 5,
                    Name = GlobalConstants.AcquiredOrderStatus,
                },
            };

            return orderStatuses;
        }

        private List<Order> DummyDataOrders()
        {
            var orders = new List<Order>
            {
                new Order
                {
                    Id = "1",
                    CommentIssuer = "Comment for the order",
                    TotalPrice = 50.49M,
                    IssuedOn = DateTime.UtcNow.AddDays(-5),
                    AddressId = 1,
                    DeliveryDate = DateTime.UtcNow.AddDays(3),
                    OrderStatusId = 1,
                    OrderStatus = new OrderStatus
                    {
                        Name = "Pending",
                    },
                    FullNameIssuer = "Ivan Ivanov",
                    Address = new Address
                    {
                        City = "Sofia",
                        StreetAddress = "Street Dondukov",
                        CityCode = 1336,
                    },
                    Courier = new ApplicationUser
                    {
                        FirstName = "Ivo",
                        LastName = "Ivov",
                    },
                    Issuer = new ApplicationUser
                    {
                        Email = "IvanIvanov@email.com",
                    },
                    IssuerId = "1",
                    PhoneNumber = "0123456789",
                },
                new Order
                {
                    Id = "2",
                    IssuedOn = DateTime.UtcNow.AddDays(-3),
                    DeliveryDate = DateTime.UtcNow.AddDays(5),
                    TotalPrice = 9.99M,
                    OrderStatusId = 4,
                    IssuerId = "1",
                    OrderStatus = new OrderStatus
                    {
                        Name = "Delivering",
                    },
                    FullNameIssuer = "Gosho Goshev",
                    Address = new Address
                    {
                        City = "Plovdiv",
                        StreetAddress = "Street Plovdivska",
                        CityCode = 7777,
                    },
                    Courier = new ApplicationUser
                    {
                        FirstName = "Peter",
                        LastName = "Petrov",
                    },
                },
                new Order
                {
                    Id = "3",
                    IssuedOn = DateTime.UtcNow.AddDays(-7),
                    DeliveryDate = DateTime.UtcNow.AddDays(1),
                    OrderStatusId = 3,
                    OrderStatus = new OrderStatus
                    {
                        Name = "Getting Groceries",
                    },
                    FullNameIssuer = "Kiro Kirev",
                    Address = new Address
                    {
                        City = "Varna",
                        StreetAddress = "Street Varnenska",
                        CityCode = 1111,
                    },
                    Courier = new ApplicationUser
                    {
                        FirstName = "Kiko",
                        LastName = "Kikev",
                    },
                    OrderRecipes = new List<OrderRecipe>
                    {
                        new OrderRecipe
                        {
                            Recipe = new Recipe
                            {
                                Name = "Musaka",
                                RecipeInstructions = "RecipeInstructions, RecipeInstructions, RecipeInstructions, RecipeInstructions, RecipeInstructions, RecipeInstructions, ",
                            },
                        },
                    },
                },
            };
            return orders;
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

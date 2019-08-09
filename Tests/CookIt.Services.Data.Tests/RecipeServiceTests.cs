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
    using Moq;

    using Xunit;

    public class RecipeServiceTests
    {
        public RecipeServiceTests()
        {
            MapperInitialize.Initialize();
        }

        [Fact]
        public async Task CreateRecipeShouldCorrectlyAddRecipe()
        {
            var recipeRepository = new Mock<IDeletableEntityRepository<Recipe>>();
            var recipeIngredientRepository = new Mock<IRepository<RecipeIngredient>>();
            var service = new RecipeService(recipeRepository.Object, recipeIngredientRepository.Object);
            var recipeModel = this.DummyDataBindingRecipe();

            var actuaResult = await service.CreateRecipe<RecipeCreateBindingModel>(recipeModel, "ImageLink");

            Assert.True(actuaResult == true, "Create Recipe should return true when created.");
            recipeRepository.Verify(x => x.AddAsync(It.IsAny<Recipe>()), Times.Once, "Create Recipe should call AddAsync method once.");
            recipeRepository.Verify(x => x.SaveChangesAsync(), Times.Once, "Create Recipe should call SaveChangesAsync method once.");
        }

        [Theory]
        [InlineData("Musaka", true)]
        [InlineData("Beef Stew", false)]
        [InlineData("Musaka2", true)]
        public void CheckRecipeByNameShouldReturnIfRecipeWithThatNameExists(string recipeName, bool expectedResult)
        {
            var recipeRepository = new Mock<IDeletableEntityRepository<Recipe>>();
            var recipeIngredientRepository = new Mock<IRepository<RecipeIngredient>>();
            recipeRepository.Setup(x => x.AllWithDeleted()).Returns(this.DummyDataRecipes().AsQueryable());
            var service = new RecipeService(recipeRepository.Object, recipeIngredientRepository.Object);

            var actualResult = service.CheckRecipeByName(recipeName);

            Assert.True(actualResult == expectedResult, $"CheckRecipeByName should return {expectedResult} with recipeName: {recipeName}");
        }

        [Fact]
        public void GetAllRecipesWithDeletedShouldReturnCorrectData()
        {
            var recipeRepository = new Mock<IDeletableEntityRepository<Recipe>>();
            var recipeIngredientRepository = new Mock<IRepository<RecipeIngredient>>();
            recipeRepository.Setup(x => x.AllWithDeleted()).Returns(this.DummyDataRecipes().AsQueryable());
            var service = new RecipeService(recipeRepository.Object, recipeIngredientRepository.Object);

            var actualResults = service.GetAllRecipesWithDeleted<RecipeAllViewModel>().ToList();
            var expectedResults = this.DummyDataRecipes();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry.Id == actualEntry.Id, "Recipe Id does not match.");
                Assert.True(expectedEntry.Name == actualEntry.Name, "Recipe Name does not match.");
                Assert.True(expectedEntry.Description == actualEntry.Description, "Recipe Description does not match.");
                Assert.True(expectedEntry.Portions == actualEntry.Portions, "Recipe Portions does not match.");
                Assert.True(expectedEntry.Price == actualEntry.Price, "Recipe Price does not match.");
                Assert.True(expectedEntry.Image == actualEntry.Image, "Recipe Image does not match.");
                Assert.True(expectedEntry.IsDeleted == actualEntry.IsDeleted, "Recipe IsDeleted does not match.");
            }

            Assert.True(actualResults.Count == expectedResults.Count);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(4, false)]

        public void CheckRecipeByIdShouldWorkProperly(int recipeId, bool expectedResult)
        {
            var recipeRepository = new Mock<IDeletableEntityRepository<Recipe>>();
            var recipeIngredientRepository = new Mock<IRepository<RecipeIngredient>>();
            recipeRepository.Setup(x => x.AllWithDeleted()).Returns(this.DummyDataRecipes().AsQueryable());
            var service = new RecipeService(recipeRepository.Object, recipeIngredientRepository.Object);

            var actualResult = service.CheckRecipeById(recipeId);

            Assert.True(actualResult == expectedResult, $"CheckRecipeById should return {expectedResult} with ID: {recipeId}");
        }

        [Fact]
        public async Task SoftDeleteRecipeShouldChangeIsDeletedToTrueCorrectly()
        {
            var recipeRepository = new Mock<IDeletableEntityRepository<Recipe>>();
            var recipeIngredientRepository = new Mock<IRepository<RecipeIngredient>>();
            var service = new RecipeService(recipeRepository.Object, recipeIngredientRepository.Object);

            var actualResult = await service.SoftDeleteRecipe(1);

            Assert.True(actualResult == true, "SoftDeleteRecipe should return true upon successfull changing to IsDeleted to true.");
            recipeRepository.Verify(x => x.Delete(It.IsAny<Recipe>()), Times.Once, "Delete method of recipe Repository should be called once.");
            recipeRepository.Verify(x => x.SaveChangesAsync(), Times.Once, "SaveChangesAsync method of recipe Repository should be called once.");
        }

        [Fact]
        public async Task UnDeleteRecipeShouldWorkCorrectly()
        {
            var recipeRepository = new Mock<IDeletableEntityRepository<Recipe>>();
            var recipeIngredientRepository = new Mock<IRepository<RecipeIngredient>>();
            var service = new RecipeService(recipeRepository.Object, recipeIngredientRepository.Object);

            var actualResult = await service.UnDeleteRecipe(1);

            Assert.True(actualResult == true, "UnDeleteRecipe should return true upon successfull changing to IsDeleted to false.");
            recipeRepository.Verify(x => x.Undelete(It.IsAny<Recipe>()), Times.Once, "UnDelete method of recipe Repository should be called once.");
            recipeRepository.Verify(x => x.SaveChangesAsync(), Times.Once, "SaveChangesAsync method of recipe Repository should be called once.");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void FindRecipeByIdShouldReturnCorrectRecipe(int recipeId)
        {
            var recipeRepository = new Mock<IDeletableEntityRepository<Recipe>>();
            var recipeIngredientRepository = new Mock<IRepository<RecipeIngredient>>();
            recipeRepository.Setup(x => x.AllWithDeleted()).Returns(this.DummyDataRecipes().AsQueryable());
            var service = new RecipeService(recipeRepository.Object, recipeIngredientRepository.Object);

            var actaulResult = service.FindRecipeById<RecipeEditBindingModel>(recipeId);
            var expectedResult = this.DummyDataRecipes().SingleOrDefault(x => x.Id == recipeId);

            Assert.True(expectedResult.Id == actaulResult.Id, "Recipe Id does not match.");
            Assert.True(expectedResult.Name == actaulResult.Name, "Recipe Name does not match.");
            Assert.True(expectedResult.Description == actaulResult.Description, "Recipe Description does not match.");
            Assert.True(expectedResult.Portions == actaulResult.Portions, "Recipe Portions does not match.");
            Assert.True(expectedResult.Price == actaulResult.Price, "Recipe Price does not match.");
            Assert.True(expectedResult.Image == actaulResult.Image, "Recipe Image does not match.");
            Assert.True(expectedResult.RecipeIngredients.Count == actaulResult.Ingredients.Count, "Recipe Ingredients Count does not match");
        }

        [Fact]
        public async Task UpdateRecipeShouldWorkProperly()
        {
            var recipeRepository = new Mock<IDeletableEntityRepository<Recipe>>();
            var recipeIngredientRepository = new Mock<IRepository<RecipeIngredient>>();
            var service = new RecipeService(recipeRepository.Object, recipeIngredientRepository.Object);

            var actualResult = await service.UpdateRecipe<RecipeEditBindingModel>(new RecipeEditBindingModel { Id = 1 }, 1);

            Assert.True(actualResult == true, "Update recipe should return true when successfully updating recipe.");

            recipeIngredientRepository.Verify(x => x.DeleteAll(It.IsAny<List<RecipeIngredient>>()), Times.Once, "DeleteAll method of RecipeIngredientRepository should be called once.");
            recipeIngredientRepository.Verify(x => x.SaveChangesAsync(), Times.Once, "SaveChangesAsync method of RecipeIngredientRepository should be called once.");

            recipeRepository.Verify(x => x.Update(It.IsAny<Recipe>()), Times.Once, "Update method of Recipe Repository should be call once.");
            recipeRepository.Verify(x => x.SaveChangesAsync(), Times.Once, "SaveChangesAsync method of Recipe Repository should be call once.");
        }

        [Fact]
        public void GetAllRecipesWithoutDeletedShouldWorkProperly()
        {
            var recipeRepository = new Mock<IDeletableEntityRepository<Recipe>>();
            var recipeIngredientRepository = new Mock<IRepository<RecipeIngredient>>();
            recipeRepository.Setup(x => x.All()).Returns(this.DummyDataRecipes().Where(x => x.IsDeleted == false).AsQueryable());
            var service = new RecipeService(recipeRepository.Object, recipeIngredientRepository.Object);

            var actualResults = service.GetAllRecipesWithoutDeleted<RecipeIndexViewModel>().ToList();
            var expectedResults = this.DummyDataRecipes().Where(x => x.IsDeleted == false).ToList();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry.Id == actualEntry.Id, "Recipe Id does not match.");
                Assert.True(expectedEntry.Name == actualEntry.Name, "Recipe Name does not match.");
                Assert.True(expectedEntry.Description == actualEntry.Description, "Recipe Description does not match.");
                Assert.True(expectedEntry.Price == actualEntry.Price, "Recipe Price does not match.");
                Assert.True(expectedEntry.Image == actualEntry.Image, "Recipe Image does not match.");
                Assert.True(expectedEntry.RecipeIngredients.Count == actualEntry.RequiredIngredients, "Recipe Ingredients Count does not match.");
                Assert.True(expectedEntry.Reviews.Count == actualEntry.ReviewsCount, "Recipe Reviews Count does not match.");
            }
        }

        [Fact]
        public void GetRecipeWithoutDeletedShouldWorkProperly()
        {
            var recipeRepository = new Mock<IDeletableEntityRepository<Recipe>>();
            var recipeIngredientRepository = new Mock<IRepository<RecipeIngredient>>();
            recipeRepository.Setup(x => x.All()).Returns(this.DummyDataRecipes().Where(x => x.IsDeleted == false).AsQueryable());
            var service = new RecipeService(recipeRepository.Object, recipeIngredientRepository.Object);

            var actualResult = service.GetRecipeWithoutDeleted<RecipeDetailsViewModel>(1);
            var expectedResult = this.DummyDataRecipes().SingleOrDefault(x => x.Id == 1);

            Assert.True(actualResult.Id == expectedResult.Id, "Recipe Id does not match.");
            Assert.True(actualResult.Name == expectedResult.Name, "Recipe Name does not match.");
            Assert.True(actualResult.Description == expectedResult.Description, "Recipe Description does not match.");
            Assert.True(actualResult.Price == expectedResult.Price, "Recipe Price does not match.");
            Assert.True(actualResult.Image == expectedResult.Image, "Recipe Image does not match.");
            Assert.True(actualResult.Ingredients.Count == expectedResult.RecipeIngredients.Count, "Recipe Ingredients Count does not match.");
            Assert.True(actualResult.Reviews.Count == expectedResult.Reviews.Count, "Recipe Reviews Count does not match.");
        }

        [Fact]
        public void GetHighestRatingsRecipesShouldWorkProperly()
        {
            var recipeRepository = new Mock<IDeletableEntityRepository<Recipe>>();
            var recipeIngredientRepository = new Mock<IRepository<RecipeIngredient>>();
            recipeRepository.Setup(x => x.All()).Returns(this.DummyDataRecipes()
                .Where(x => x.IsDeleted == false)
                .AsQueryable());
            var service = new RecipeService(recipeRepository.Object, recipeIngredientRepository.Object);

            var actualResults = service.GetHighestRatingsRecipes<RecipeBestRatingsViewModel>().ToList();
            var expectedResults = this.DummyDataRecipes().Where(x => x.IsDeleted == false).OrderByDescending(c => (c.Reviews.Sum(z => z.Stars) * 1.0) / c.Reviews.Count).ToList();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry.Id == actualEntry.Id, "Recipe Id does not match.");
                Assert.True(expectedEntry.Name == actualEntry.Name, "Recipe Name does not match.");
                Assert.True(expectedEntry.Image == actualEntry.Image, "Recipe Image does not match.");
                Assert.True(expectedEntry.Reviews.Sum(z => z.Stars) * 1.0 / expectedEntry.Reviews.Count == actualEntry.Rating, "Racipe Review Rating does not match.");
            }
        }

        [Fact]
        public void GetLatestRecipesShouldWorkProperly()
        {
            var recipeRepository = new Mock<IDeletableEntityRepository<Recipe>>();
            var recipeIngredientRepository = new Mock<IRepository<RecipeIngredient>>();
            recipeRepository.Setup(x => x.All()).Returns(this.DummyDataRecipes()
                .AsQueryable());
            var service = new RecipeService(recipeRepository.Object, recipeIngredientRepository.Object);

            var actualResults = service.GetLatestRecipes<RecipeLatestViewModel>().ToList();
            var expectedResults = this.DummyDataRecipes().OrderByDescending(x => x.Id).ToList();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry.Id == actualEntry.Id, "Recipe Id does not match.");
                Assert.True(expectedEntry.Name == actualEntry.Name, "Recipe Name does not match.");
                Assert.True(expectedEntry.Image == actualEntry.Image, "Recipe Image does not match.");
                Assert.True(expectedEntry.Price == actualEntry.Price, "Recipe Price does not match.");
            }
        }

        private List<Recipe> DummyDataRecipes()
        {
            var recipes = new List<Recipe>
            {
                new Recipe
                {
                Id = 1,
                Name = "Musaka",
                Description = "Musaka Description",
                Portions = 4,
                Price = 4.99M,
                RecipeInstructions = "Musaka InstructionsMusaka InstructionsMusaka InstructionsMusaka Instructions",
                Image = "Image Url Link",
                IsDeleted = false,
                RecipeIngredients = new List<RecipeIngredient>
                {
                    new RecipeIngredient
                    {
                        Count = 5,
                        Weight = null,
                        Ingredient = new Ingredient
                        {
                            Name = "Beef",
                        },
                    },
                    new RecipeIngredient
                    {
                        Count = null,
                        Weight = 3.5,
                        Ingredient = new Ingredient
                        {
                            Name = "Cucumber",
                        },
                    },
                },
                Reviews = new List<Review>
                {
                    new Review
                    {
                        Stars = 5,
                        Content = "Review Content Review Content",
                        CreatedOn = DateTime.UtcNow.AddDays(-5),
                        ApplicationUser = new ApplicationUser
                        {
                            FirstName = "Pesho",
                            LastName = "Petrov",
                        },
                    },
                    new Review
                    {
                        Stars = 4,
                        Content = "Review Content Review Content",
                        CreatedOn = DateTime.UtcNow.AddDays(-3),
                        ApplicationUser = new ApplicationUser
                        {
                            FirstName = "Gosho",
                            LastName = "Ivanov",
                        },
                    },
                },
                },
                new Recipe
                {
                Id = 2,
                Name = "Musaka2",
                Description = "Musaka2 Description",
                Portions = 2,
                Price = 8.99M,
                RecipeInstructions = "Musaka2 InstructionsMusaka2 InstructionsMusaka2 InstructionsMusaka2 Instructions",
                Image = "Image2 Url Link",
                IsDeleted = false,
                Reviews = new List<Review>
                {
                    new Review
                    {
                        Stars = 5,
                        Content = "Review Content Review Content",
                        CreatedOn = DateTime.UtcNow.AddDays(-5),
                        ApplicationUser = new ApplicationUser
                        {
                            FirstName = "Pesho",
                            LastName = "Petrov",
                        },
                    },
                    new Review
                    {
                        Stars = 5,
                        Content = "Review Content Review Content",
                        CreatedOn = DateTime.UtcNow.AddDays(-3),
                        ApplicationUser = new ApplicationUser
                        {
                            FirstName = "Gosho",
                            LastName = "Ivanov",
                        },
                    },
                },
                },
                new Recipe
                {
                Id = 3,
                Name = "Musaka3",
                Description = "Musaka3 Description",
                Portions = 3,
                Price = 10.99M,
                RecipeInstructions = "Musaka3 InstructionsMusaka3 InstructionsMusaka3 InstructionsMusaka3 Instructions",
                Image = "Image3 Url Link",
                IsDeleted = true,
                },
            };
            return recipes;
        }

        private RecipeCreateBindingModel DummyDataBindingRecipe()
        {
            var recipe = new RecipeCreateBindingModel
            {
                Name = "Musaka",
                Description = "Musaka Description",
                Portions = 4,
                Price = 4.99M,
                RecipeInstructions = "Musaka InstructionsMusaka InstructionsMusaka InstructionsMusaka Instructions",
                Ingredients = new List<RecipeIngredientBindingModel>
                {
                    new RecipeIngredientBindingModel(),
                    new RecipeIngredientBindingModel(),
                },
            };
            return recipe;
        }
    }
}

namespace CookIt.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using CookIt.Common;
    using CookIt.Data.Common.Repositories;
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;
    using CookIt.Web.BindingModels.Address;
    using CookIt.Web.BindingModels.Ingredient;
    using CookIt.Web.ViewModels;
    using CookIt.Web.ViewModels.Address;
    using CookIt.Web.ViewModels.Administration;
    using CookIt.Web.ViewModels.Ingridient;
    using Microsoft.AspNetCore.Identity;

    using Moq;

    using Xunit;

    public class IngredientServiceTests
    {
        public IngredientServiceTests()
        {
            MapperInitialize.Initialize();
        }

        [Fact]
        public async Task CreateIngredientTypeMustCreateProperly()
        {
            var ingredientTypeRepository = new Mock<IRepository<IngredientType>>();
            var ingredientRepository = new Mock<IRepository<Ingredient>>();
            var service = new IngredientService(ingredientTypeRepository.Object, ingredientRepository.Object);

            var actualResult = await service.CreateIngredientType("Meat");

            Assert.True(actualResult == true, "Create Ingredient Type doest work properly.");
            ingredientTypeRepository.Verify(x => x.AddAsync(It.IsAny<IngredientType>()), Times.Once, "Create Ingredient Type was not called once.");
            ingredientTypeRepository.Verify(x => x.SaveChangesAsync(), Times.Once, "Create Ingredient Type saveChanges was not called once.");
        }

        [Fact]
        public void CheckIfIngredientTypeExistsByNameShouldReturnTrueIfExists()
        {
            var ingredientTypeRepository = new Mock<IRepository<IngredientType>>();
            var ingredientRepository = new Mock<IRepository<Ingredient>>();
            var service = new IngredientService(ingredientTypeRepository.Object, ingredientRepository.Object);

            ingredientTypeRepository.Setup(x => x.All()).Returns(this.DummyDataIngredientType().AsQueryable());

            var expectedResultExistingType = service.CheckIfIngredientTypeExistByName("Meat");
            var expectedResultNonExistingType = service.CheckIfIngredientTypeExistByName("Mea");

            Assert.True(expectedResultExistingType == true, "Ingredient Type Check should return True.");
            Assert.True(expectedResultNonExistingType == false, "Ingredient Type Check should return False.");
        }

        [Fact]
        public async Task RemoveIngredientTypeByIdShouldRemoveCorrectlyWithExistingId()
        {
            var ingredientTypeRepository = new Mock<IRepository<IngredientType>>();
            var ingredientRepository = new Mock<IRepository<Ingredient>>();
            var service = new IngredientService(ingredientTypeRepository.Object, ingredientRepository.Object);
            ingredientTypeRepository.Setup(x => x.All()).Returns(this.DummyDataIngredientType().AsQueryable());

            var actualResult = await service.RemoveIngredientTypeById(1);

            Assert.True(actualResult == true, "Removing type with data should return true");
            ingredientTypeRepository.Verify(x => x.Delete(It.IsAny<IngredientType>()), Times.Once, "Removing Ingredient Type should be called once.");
            ingredientTypeRepository.Verify(x => x.SaveChangesAsync(), Times.Once, "Removing Ingredient Types SaveChanges should be called once.");
        }

        [Fact]
        public async Task RemoveIngredientTypeByIdShouldRemoveCorrectlyWithNonExistintId()
        {
            var ingredientTypeRepository = new Mock<IRepository<IngredientType>>();
            var ingredientRepository = new Mock<IRepository<Ingredient>>();
            var service = new IngredientService(ingredientTypeRepository.Object, ingredientRepository.Object);
            ingredientTypeRepository.Setup(x => x.All()).Returns(this.DummyDataIngredientType().AsQueryable());

            var actualResult = await service.RemoveIngredientTypeById(3);

            Assert.True(actualResult == false, "Removing ingredient type with non existing Id should return false");
        }

        [Fact]
        public void GetIngredientTypeNameByIdWithData()
        {
            var ingredientTypeRepository = new Mock<IRepository<IngredientType>>();
            var ingredientRepository = new Mock<IRepository<Ingredient>>();
            ingredientTypeRepository.Setup(x => x.All()).Returns(this.DummyDataIngredientType().AsQueryable());
            var service = new IngredientService(ingredientTypeRepository.Object, ingredientRepository.Object);

            var actualResult = service.GetIngredientTypeNameById(1);
            var expectedResult = "Meat";

            Assert.True(actualResult == expectedResult, "Get Ingredient Type Name By Id with data should return true.");
        }

        [Fact]
        public void GetIngredientTypeNameByIdWithoutData()
        {
            var ingredientTypeRepository = new Mock<IRepository<IngredientType>>();
            var ingredientRepository = new Mock<IRepository<Ingredient>>();
            var service = new IngredientService(ingredientTypeRepository.Object, ingredientRepository.Object);

            var actualResult = service.GetIngredientTypeNameById(1);

            Assert.True(actualResult == null, "Get Ingredient Type Name By Id without data should return true.");
        }

        [Fact]
        public void GetAllIngredientTypesWithData()
        {
            var ingredientTypeRepository = new Mock<IRepository<IngredientType>>();
            var ingredientRepository = new Mock<IRepository<Ingredient>>();
            ingredientTypeRepository.Setup(x => x.All()).Returns(this.DummyDataIngredientType().AsQueryable());
            var service = new IngredientService(ingredientTypeRepository.Object, ingredientRepository.Object);

            var actualResults = service.GetAllIngreientTypes<AllIngredientTypeViewModel>().ToList();
            var expectedResults = this.DummyDataIngredientType();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry.Name == actualEntry.Name, "Ingredient Type Name does not match.");
                Assert.True(expectedEntry.Id == actualEntry.Id, "Ingredient Type Id does not match.");
            }
        }

        [Fact]
        public void GetAllIngredientTypesWithoutData()
        {
            var ingredientTypeRepository = new Mock<IRepository<IngredientType>>();
            var ingredientRepository = new Mock<IRepository<Ingredient>>();
            ingredientTypeRepository.Setup(x => x.All()).Returns(new List<IngredientType>().AsQueryable());
            var service = new IngredientService(ingredientTypeRepository.Object, ingredientRepository.Object);

            var actualResults = service.GetAllIngreientTypes<AllIngredientTypeViewModel>().ToList();

            Assert.True(actualResults.Count == 0, "GetAllIngredientTypesWithoutData should return Empty collection.");
        }

        [Fact]
        public void CheckIfIngredientTypeExistByIdShouldWorkProperly()
        {
            var ingredientTypeRepository = new Mock<IRepository<IngredientType>>();
            var ingredientRepository = new Mock<IRepository<Ingredient>>();
            ingredientTypeRepository.Setup(x => x.All()).Returns(this.DummyDataIngredientType().AsQueryable());
            var service = new IngredientService(ingredientTypeRepository.Object, ingredientRepository.Object);

            var actualResultWithExistingData = service.CheckIfIngredientTypeExistById(1);
            var actualResullWithNonExistingData = service.CheckIfIngredientTypeExistById(5);

            Assert.True(actualResultWithExistingData == true, "CheckIfIngredientTypeExistById with correct data should return True.");
            Assert.True(actualResullWithNonExistingData == false, "CheckIfIngredientTypeExistById with non existing data should return False.");
        }

        [Fact]
        public async Task CreateIngredientShouldProperlyWork()
        {
            var ingredientTypeRepository = new Mock<IRepository<IngredientType>>();
            var ingredientRepository = new Mock<IRepository<Ingredient>>();
            var service = new IngredientService(ingredientTypeRepository.Object, ingredientRepository.Object);

            var actualResult = await service.CreateIngredient<IngredientBindingModel>(new IngredientBindingModel { Name = "Carrots" });

            ingredientRepository.Verify(x => x.AddAsync(It.IsAny<Ingredient>()), Times.Once, "Create ingredients should call AddAsync Once with correct type.");
            Assert.True(actualResult == true, "Create ingredient should return true with correct data");
        }

        [Fact]
        public void GetAllIngredientsShouldReturnDataCorrectly()
        {
            var ingredientTypeRepository = new Mock<IRepository<IngredientType>>();
            var ingredientRepository = new Mock<IRepository<Ingredient>>();
            ingredientRepository.Setup(x => x.All()).Returns(this.DummyDataIngredient().AsQueryable());
            var service = new IngredientService(ingredientTypeRepository.Object, ingredientRepository.Object);

            var actualResults = service.GetAllIngreients<AllIngredientViewModel>().ToList();
            var expectedResults = this.DummyDataIngredient();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry.Name == actualEntry.Name, "Ingredient Name does not match.");
                Assert.True(expectedEntry.Id == actualEntry.Id, "Ingredient Id does not match.");
                Assert.True(expectedEntry.IngredientType.Name == actualEntry.IngredientTypeName, "Ingredient Type Name does not match.");
            }
        }

        [Theory]
        [InlineData(1, "Carrots")]
        [InlineData(3, null)]
        public void GetIngredientNameByIdShouldReturnNameProperly(int ingredientId, string expectedResult)
        {
            var ingredientTypeRepository = new Mock<IRepository<IngredientType>>();
            var ingredientRepository = new Mock<IRepository<Ingredient>>();
            ingredientRepository.Setup(x => x.All()).Returns(this.DummyDataIngredient().AsQueryable());
            var service = new IngredientService(ingredientTypeRepository.Object, ingredientRepository.Object);

            var actualResult = service.GetIngredientNameById(ingredientId);

            Assert.True(actualResult == expectedResult, "Get Ingredient Name By Id should return correct Name");
        }

        [Theory]
        [InlineData("Carrots", true)]
        [InlineData("Beef", false)]
        public void IngredientExistsByNameShouldReturnTrueWithValidDataAndFalseWithNonExistingIngredientName(string name, bool expectedResult)
        {
            var ingredientTypeRepository = new Mock<IRepository<IngredientType>>();
            var ingredientRepository = new Mock<IRepository<Ingredient>>();
            ingredientRepository.Setup(x => x.All()).Returns(this.DummyDataIngredient().AsQueryable());
            var service = new IngredientService(ingredientTypeRepository.Object, ingredientRepository.Object);

            var actualResult = service.IngredientExistsByName(name);

            Assert.True(actualResult == expectedResult, "IngredientExistsByName should return true if ingredient exists and false if it doesn't.");
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(4, false)]
        public async Task RemoveIngredientByIdShouldWorkCorrectly(int ingredientId, bool expectedResult)
        {
            var ingredientTypeRepository = new Mock<IRepository<IngredientType>>();
            var ingredientRepository = new Mock<IRepository<Ingredient>>();
            ingredientRepository.Setup(x => x.All()).Returns(this.DummyDataIngredient().AsQueryable());
            var service = new IngredientService(ingredientTypeRepository.Object, ingredientRepository.Object);

            var actualResult = await service.RemoveIngredientById(ingredientId);

            Assert.True(actualResult == expectedResult, $"Remove ingredient by id should return {expectedResult} with Id: {ingredientId}.");
            if (actualResult)
            {
                ingredientRepository.Verify(x => x.Delete(It.IsAny<Ingredient>()), Times.Once, "Delete method of the ingredient repository should be called once.");
            }
        }

        [Theory]
        [InlineData(1, 2, true)]
        [InlineData(1, 3, false)]
        public void CheckExistingIngredientIdShouldWorkProperly(int firstId, int secondId, bool expectedResult)
        {
            var ingredientTypeRepository = new Mock<IRepository<IngredientType>>();
            var ingredientRepository = new Mock<IRepository<Ingredient>>();
            ingredientRepository.Setup(x => x.All()).Returns(this.DummyDataIngredient().AsQueryable());
            var service = new IngredientService(ingredientTypeRepository.Object, ingredientRepository.Object);

            var ids = new List<int> { firstId, secondId };
            var actualResult = service.CheckExistingIngredientId(ids);

            Assert.True(actualResult == expectedResult, $"Check Existing Ingredients Id should return {expectedResult} with Data: {firstId}, {secondId}.");
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, false)]
        public void HasIngredientWithTypeShouldWorkProperly(int ingredientTypeId, bool expectedResult)
        {
            var ingredientTypeRepository = new Mock<IRepository<IngredientType>>();
            var ingredientRepository = new Mock<IRepository<Ingredient>>();
            ingredientTypeRepository.Setup(x => x.All()).Returns(this.DummyDataIngredientType().AsQueryable());
            var service = new IngredientService(ingredientTypeRepository.Object, ingredientRepository.Object);

            var actualResult = service.HasIngredientWithType(ingredientTypeId);

            Assert.True(actualResult == expectedResult, $"Has Ingredient with specific type should return {expectedResult} with IngredientTypeId: {ingredientTypeId}");
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, false)]
        public void HasRecipeWithIngredientShouldWorkProperly(int ingredientId, bool expectedResult)
        {
            var ingredientTypeRepository = new Mock<IRepository<IngredientType>>();
            var ingredientRepository = new Mock<IRepository<Ingredient>>();
            ingredientRepository.Setup(x => x.All()).Returns(this.DummyDataIngredient().AsQueryable());
            var service = new IngredientService(ingredientTypeRepository.Object, ingredientRepository.Object);

            var actualResult = service.HasRecipeWithIngredient(ingredientId);

            Assert.True(actualResult == expectedResult, $"HasRecipeWithIngredient should return {expectedResult} with ingredientId: {ingredientId}");
        }

        private List<IngredientType> DummyDataIngredientType()
        {
            var ingredientTypes = new List<IngredientType>
            {
                new IngredientType
                {
                    Id = 1,
                    Name = "Meat",
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient
                        {
                            Id = 1,
                        },
                        new Ingredient
                        {
                            Id = 2,
                        },
                    },
                },
                new IngredientType
                {
                    Id = 2,
                    Name = "Fruit",
                },
            };

            return ingredientTypes;
        }

        private List<Ingredient> DummyDataIngredient()
        {
            var ingredients = new List<Ingredient>
            {
                new Ingredient
                {
                    Id = 1,
                    IngredientType = new IngredientType
                    {
                        Name = "Vegetables",
                    },
                    Name = "Carrots",
                    RecipeIngredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient(),
                        new RecipeIngredient(),
                    },
                },
                new Ingredient
                {
                    Id = 2,
                    IngredientType = new IngredientType
                    {
                        Name = "Vegetables",
                    },
                    Name = "Tomatos",
                },
            };
            return ingredients;
        }
    }
}

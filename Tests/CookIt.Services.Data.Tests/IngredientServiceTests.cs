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
    using CookIt.Web.ViewModels;
    using CookIt.Web.ViewModels.Address;
    using CookIt.Web.ViewModels.Administration;
    using Microsoft.AspNetCore.Identity;

    using Moq;

    using Xunit;

    public class IngredientServiceTests
    {
        [Fact]
        public async Task CreateIngredientTypeMustCreateProperly()
        {
            var ingredientTypeRepository = new Mock<IRepository<IngredientType>>();
            var ingredientRepository = new Mock<IRepository<Ingredient>>();
            var service = new IngredientService(ingredientTypeRepository.Object, ingredientRepository.Object);

            var actualResult = await service.CreateIngredientType("Meat");

            Assert.True(actualResult == true,  "Create Ingredient Type doest work properly.");
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

        private List<IngredientType> DummyDataIngredientType()
        {
            var ingredientTypes = new List<IngredientType>
            {
                new IngredientType
                {
                    Id = 1,
                    Name = "Meat",
                },
                new IngredientType
                {
                    Id = 2,
                    Name = "Fruit",
                },
            };

            return ingredientTypes;
        }
    }
}

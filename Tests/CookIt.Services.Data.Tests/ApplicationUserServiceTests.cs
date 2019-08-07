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

    public class ApplicationUserServiceTests
    {
        [Fact]
        public async Task AddAddressShouldAddAddressCorectly()
        {
            this.InitializeMapper();
            var addressRepository = new Mock<IRepository<Address>>();
            var userRepository = new Mock<IRepository<ApplicationUser>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            userRepository.Setup(r => r.All()).Returns(new List<ApplicationUser>
            {
                new ApplicationUser { FirstName = "root" },
            }.AsQueryable());

            AddressBindingModel address = new AddressBindingModel();

            var user = userRepository.Object.All().SingleOrDefault(x => x.FirstName == "root");
            var service = new ApplicationUserService(userRepository.Object, addressRepository.Object, userManager.Object);
            await service.AddAddress<AddressBindingModel>(address, user.Id);

            Assert.Single(userRepository.Object.All().SingleOrDefault(x => x.FirstName == "root").Addresses);
        }

        [Fact]

        public void GetAllAddressesByUserShouldReturnCorrectResults()
        {
            this.InitializeMapper();
            var addressRepository = new Mock<IRepository<Address>>();
            var userRepository = new Mock<IRepository<ApplicationUser>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            userRepository.Setup(r => r.All()).Returns(this.DummyDataForUser().AsQueryable());
            var user = userRepository.Object.All().SingleOrDefault(x => x.FirstName == "root");
            addressRepository.Setup(r => r.All()).Returns(this.DummyDataAddresses(user.Id).AsQueryable());

            var service = new ApplicationUserService(userRepository.Object, addressRepository.Object, userManager.Object);
            var actualResults = service.GetAllAddresses<AddressViewModel>(user.Id).ToList();
            var expectedResults = this.DummyDataAddresses(user.Id).ToList();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry.City == actualEntry.City, "City does not match");
                Assert.True(expectedEntry.Description == actualEntry.Description, "Description does not match");
                Assert.True(expectedEntry.CityCode == actualEntry.CityCode, "CityCode does not match");
                Assert.True(expectedEntry.StreetAddress == actualEntry.StreetAddress, "StreetAddress does not match");
            }
        }

        [Fact]
        public async Task RemoveAddressShouldCorrectlyRemoveAddressById()
        {
            this.InitializeMapper();
            var addressRepository = new Mock<IRepository<Address>>();
            var userRepository = new Mock<IRepository<ApplicationUser>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            addressRepository.Setup(r => r.All()).Returns(this.DummyDataAddresses("1").AsQueryable());
            var service = new ApplicationUserService(userRepository.Object, addressRepository.Object, userManager.Object);
            await service.RemoveAddressById(1);

            var entity = addressRepository.Object.All().SingleOrDefault(x => x.Id == 1);
            addressRepository.Verify(x => x.Delete(entity), Times.Once, "Removing address does not work properly.");
        }

        [Fact]
        public async Task GetUsersByRoleNameShouldReturnCorrectData()
        {
            this.InitializeMapper();
            var addressRepository = new Mock<IRepository<Address>>();
            var userRepository = new Mock<IRepository<ApplicationUser>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            var service = new ApplicationUserService(userRepository.Object, addressRepository.Object, userManager.Object);

            var result = await service.GetUsersByRoleName<AdminViewModel>(GlobalConstants.AdministratorRoleName);

            userManager.Verify(x => x.GetUsersInRoleAsync(GlobalConstants.AdministratorRoleName), Times.Once, "Getting Users by Role does not work properly.");
        }

        private void InitializeMapper()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly, typeof(AddressBindingModel).GetTypeInfo().Assembly);
        }

        private IEnumerable<ApplicationUser> DummyDataForUser()
        {
            var user = new ApplicationUser
            {
                FirstName = "root",
            };
            var users = new List<ApplicationUser> { user };
            return users;
        }

        private IEnumerable<Address> DummyDataAddresses(string userId)
        {
            var addresses = new List<Address>
                    {
                        new Address
                        {
                            Id = 1,
                            ApplicationUserId = userId,
                            City = "Sofia",
                            CityCode = 1337,
                            StreetAddress = "street 291",
                            Description = "go left",
                        },
                        new Address
                        {
                            Id = 2,
                            ApplicationUserId = userId,
                            City = "Varna",
                            CityCode = 1337,
                            StreetAddress = "street 291",
                            Description = "go left",
                        },
                        new Address
                        {
                            Id = 3,
                            ApplicationUserId = userId,
                            City = "Burgas",
                            CityCode = 1337,
                            StreetAddress = "street 291",
                            Description = "go left",
                        },
                    };

            return addresses;
        }
    }
}

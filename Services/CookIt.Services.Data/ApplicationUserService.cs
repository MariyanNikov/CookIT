namespace CookIt.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using CookIt.Data.Common.Repositories;
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IRepository<ApplicationUser> userRepository;
        private readonly IRepository<Address> addressesRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public ApplicationUserService(
            IRepository<ApplicationUser> userRepository,
            IRepository<Address> addressesRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.userRepository = userRepository;
            this.addressesRepository = addressesRepository;
            this.userManager = userManager;
        }

        public async Task AddAddress<TModel>(TModel addressBindingModel, string userId)
        {
            var address = Mapper.Map<Address>(addressBindingModel);

            var user = this.userRepository.All().SingleOrDefault(x => x.Id == userId);
            user.Addresses.Add(address);

            await this.userRepository.SaveChangesAsync();
        }

        public IEnumerable<TModel> GetAllAddresses<TModel>(string userId)
        {
            var addresses = this.addressesRepository.All().Where(x => x.ApplicationUserId == userId).To<TModel>().ToList();

            return addresses;
        }

        public async Task<IEnumerable<TModel>> GetUsersByRoleName<TModel>(string roleName)
        {
            var users = Mapper.Map<IEnumerable<TModel>>(await this.userManager.GetUsersInRoleAsync(roleName));

            return users;
        }

        public async Task RemoveAddressById(int addressId)
        {
            var address = this.addressesRepository.All().SingleOrDefault(x => x.Id == addressId);

            this.addressesRepository.Delete(address);
            await this.addressesRepository.SaveChangesAsync();
        }
    }
}

namespace CookIt.Services.Data.ApplicationUser
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IApplicationUserService
    {
        Task AddAddress<TModel>(TModel addressBindingModel, string userId);

        IEnumerable<TModel> GetAllAddresses<TModel>(string userId);

        Task RemoveAddressById(int addressId);
    }
}

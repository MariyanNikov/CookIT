namespace CookIt.Web.ViewModels.Address
{
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;

    public class AddressViewModel : IMapFrom<Address>
    {
        public int Id { get; set; }

        public string StreetAddress { get; set; }

        public string Description { get; set; }

        public string City { get; set; }

        public int CityCode { get; set; }
    }
}

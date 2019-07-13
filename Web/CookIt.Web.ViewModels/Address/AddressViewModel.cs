namespace CookIt.Web.ViewModels.Address
{
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;

    public class AddressViewModel : IMapFrom<Addresses>
    {
        public int Id { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public string City { get; set; }

        public int CityCode { get; set; }
    }
}

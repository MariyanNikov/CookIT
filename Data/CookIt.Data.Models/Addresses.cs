namespace CookIt.Data.Models
{
    using CookIt.Data.Common.Models;

    public class Addresses : BaseModel<int>
    {
        public string Address { get; set; }

        public string Description { get; set; }

        public string City { get; set; }

        public int CityCode { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}

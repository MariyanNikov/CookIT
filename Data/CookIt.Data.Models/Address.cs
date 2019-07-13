namespace CookIt.Data.Models
{
    using CookIt.Data.Common.Models;

    public class Address : BaseModel<int>
    {
        public string StreetAddress { get; set; }

        public string Description { get; set; }

        public string City { get; set; }

        public int CityCode { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}

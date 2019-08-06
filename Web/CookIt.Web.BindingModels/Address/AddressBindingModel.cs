namespace CookIt.Web.BindingModels.Address
{
    using System.ComponentModel.DataAnnotations;

    using CookIt.Data.Models;
    using CookIt.Services.Mapping;

    public class AddressBindingModel : IMapTo<Address>
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        public string Description { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string City { get; set; }

        public int CityCode { get; set; }
    }
}

namespace CookIt.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CookIt.Data.Common.Models;

    public class Address : BaseModel<int>
    {
        public Address()
        {
            this.Orders = new HashSet<Order>();
        }

        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string StreetAddress { get; set; }

        public string Description { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string City { get; set; }

        public int CityCode { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}

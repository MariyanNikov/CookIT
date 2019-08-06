namespace CookIt.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CookIt.Data.Common.Models;

    public class Order : BaseModel<string>
    {
        public Order()
        {
            this.OrderRecipes = new HashSet<OrderRecipe>();
        }

        [Required]
        public DateTime DeliveryDate { get; set; }

        public DateTime IssuedOn { get; set; }

        [Required]
        [StringLength(61, MinimumLength = 7)]
        public string FullNameIssuer { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [StringLength(255, MinimumLength = 10)]
        public string CommentIssuer { get; set; }

        public decimal TotalPrice { get; set; }

        [Required]
        public int AddressId { get; set; }

        public Address Address { get; set; }

        [Required]
        public int OrderStatusId { get; set; }

        public OrderStatus OrderStatus { get; set; }

        [Required]
        public string IssuerId { get; set; }

        public ApplicationUser Issuer { get; set; }

        public string CourierId { get; set; }

        public ApplicationUser Courier { get; set; }

        public ICollection<OrderRecipe> OrderRecipes { get; set; }
    }
}

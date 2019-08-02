namespace CookIt.Data.Models
{
    using System;
    using System.Collections.Generic;

    using CookIt.Data.Common.Models;

    public class Order : BaseModel<string>
    {
        public Order()
        {
            this.OrderRecipes = new HashSet<OrderRecipe>();
        }

        public DateTime DeliveryDate { get; set; }

        public DateTime IssuedOn { get; set; }

        public string FullNameIssuer { get; set; }

        public string PhoneNumber { get; set; }

        public string CommentIssuer { get; set; }

        public decimal TotalPrice { get; set; }

        public int AddressId { get; set; }

        public Address Address { get; set; }

        public int OrderStatusId { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public string IssuerId { get; set; }

        public ApplicationUser Issuer { get; set; }

        public string CourierId { get; set; }

        public ApplicationUser Courier { get; set; }

        public ICollection<OrderRecipe> OrderRecipes { get; set; }
    }
}

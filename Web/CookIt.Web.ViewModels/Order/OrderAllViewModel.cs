namespace CookIt.Web.ViewModels.Order
{
    using System;

    using CookIt.Data.Models;
    using CookIt.Services.Mapping;

    public class OrderAllViewModel : IMapFrom<Order>
    {
        public string Id { get; set; }

        public DateTime IssuedOn { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string OrderStatusName { get; set; }

        public string FullNameIssuer { get; set; }
    }
}

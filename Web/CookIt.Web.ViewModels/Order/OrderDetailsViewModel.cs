namespace CookIt.Web.ViewModels.Order
{
    using System;

    using AutoMapper;
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;

    public class OrderDetailsViewModel : IMapFrom<Order>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public DateTime IssuedOn { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string FullNameIssuer { get; set; }

        public string PhoneNumber { get; set; }

        public string CommentIssuer { get; set; }

        public decimal TotalPrice { get; set; }

        public string DeliveryAddress { get; set; }

        public string OrderStatusName { get; set; }

        public string CourierName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, OrderDetailsViewModel>()
                .ForMember(x => x.DeliveryAddress, opt => opt.MapFrom(c => string.Join(" - ", c.Address.StreetAddress, c.Address.City, c.Address.CityCode)))
                .ForMember(x => x.CourierName, opt => opt.MapFrom(c => c.Courier.FirstName + " " + c.Courier.LastName));
        }
    }
}

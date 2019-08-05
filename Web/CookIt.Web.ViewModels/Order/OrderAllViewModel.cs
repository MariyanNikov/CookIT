namespace CookIt.Web.ViewModels.Order
{
    using System;

    using AutoMapper;
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;

    public class OrderAllViewModel : IMapFrom<Order>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public DateTime IssuedOn { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string OrderStatusName { get; set; }

        public string FullNameIssuer { get; set; }

        public string Address { get; set; }

        public string CourierId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, OrderAllViewModel>()
           .ForMember(x => x.Address, opt => opt.MapFrom(c => string.Join(" - ", c.Address.StreetAddress, c.Address.City, c.Address.CityCode)));
        }
    }
}

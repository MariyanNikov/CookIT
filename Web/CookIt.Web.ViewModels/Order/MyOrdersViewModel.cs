namespace CookIt.Web.ViewModels.Order
{
    using System;

    using AutoMapper;
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;

    public class MyOrdersViewModel : IMapFrom<Order>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string OrderStatusName { get; set; }

        public decimal TotalPrice { get; set; }

        public string Courier { get; set; }

        public string DeliveryAddress { get; set; }

        public DateTime DeliveryDate { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, MyOrdersViewModel>()
                .ForMember(x => x.DeliveryAddress, opt => opt.MapFrom(c => string.Join(" - ", c.Address.StreetAddress, c.Address.City, c.Address.CityCode)))
                .ForMember(x => x.Courier, opt => opt.MapFrom(c => c.Courier.FirstName + " " + c.Courier.LastName));
        }
    }
}

namespace CookIt.Web.ViewModels.Administration
{
    using AutoMapper;
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;

    public class AdminViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, AdminViewModel>().
                ForMember(
                m => m.FullName,
                opt => opt.MapFrom(x => x.FirstName + " " + x.LastName));
        }
    }
}

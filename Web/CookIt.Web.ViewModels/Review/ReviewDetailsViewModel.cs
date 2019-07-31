namespace CookIt.Web.ViewModels.Review
{
    using AutoMapper;
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;

    public class ReviewDetailsViewModel : IMapFrom<Review>, IHaveCustomMappings
    {
        public string Content { get; set; }

        public int Stars { get; set; }

        public string Author { get; set; }

        public string CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Review, ReviewDetailsViewModel>()
                .ForMember(x => x.Author, opt => opt.MapFrom(c => c.ApplicationUser.FirstName + " " + c.ApplicationUser.LastName))
                .ForMember(x => x.CreatedOn, opt => opt.MapFrom(c => c.CreatedOn.ToString("G")));
        }
    }
}

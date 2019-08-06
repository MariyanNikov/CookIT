namespace CookIt.Web.ViewModels.Recipe
{
    using System.Linq;

    using AutoMapper;
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;

    public class RecipeBestRatingsViewModel : IMapFrom<Recipe>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Image { get; set; }

        public string Name { get; set; }

        public double Rating { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Recipe, RecipeBestRatingsViewModel>()
                .ForMember(x => x.Rating, opt => opt.MapFrom(c => c.Reviews.Sum(z => z.Stars) * 1.0 / c.Reviews.Count));
        }
    }
}

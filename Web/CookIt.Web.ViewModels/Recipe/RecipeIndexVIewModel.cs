namespace CookIt.Web.ViewModels.Recipe
{
    using AutoMapper;
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;

    public class RecipeIndexViewModel : IMapFrom<Recipe>, IHaveCustomMappings
    {
        private const int NumberOfCharsForDescription = 100;

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int RequiredIngredients { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }

        public int ReviewsCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Recipe, RecipeIndexViewModel>()
                .ForMember(x => x.RequiredIngredients, opt => opt.MapFrom(c => c.RecipeIngredients.Count))
                .ForMember(x => x.Description, opt => opt.AddTransform(c => c.Length > NumberOfCharsForDescription ? c.Substring(0, NumberOfCharsForDescription) + "..." : c.ToString()))
                .ForMember(x => x.ReviewsCount, opt => opt.MapFrom(c => c.Reviews.Count));
        }
    }
}

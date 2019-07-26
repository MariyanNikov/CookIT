namespace CookIt.Web.ViewModels.Recipe
{
    using AutoMapper;
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;

    public class RecipeAllIngredientsViewModel : IMapFrom<RecipeIngredient>, IHaveCustomMappings
    {
        public string Name { get; set; }

        public int? Count { get; set; }

        public double? Weight { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<RecipeIngredient, RecipeAllIngredientsViewModel>()
                 .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Ingredient.Name))
                 .ForMember(x => x.Count, opt => opt.MapFrom(c => c.Count))
                 .ForMember(x => x.Weight, opt => opt.MapFrom(c => c.Weight));
        }
    }
}

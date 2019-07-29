namespace CookIt.Web.ViewModels.Recipe
{
    using AutoMapper;
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;

    public class RecipeIngredientsDetailsViewModel : IMapFrom<RecipeIngredient>, IHaveCustomMappings
    {
        private const string WeightSuffix = " / kg";

        public string Name { get; set; }

        public string Amount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<RecipeIngredient, RecipeIngredientsDetailsViewModel>()
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Ingredient.Name))
                .ForMember(x => x.Amount, opt => opt.MapFrom(c => c.Count.HasValue ? c.Count.Value.ToString() : (c.Weight.Value / 1000).ToString("F3") + WeightSuffix));
        }
    }
}

namespace CookIt.Web.ViewModels.Recipe
{
    using System.Collections.Generic;

    using AutoMapper;
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;

    public class RecipeAllViewModel : IMapFrom<Recipe>, IHaveCustomMappings
    {
        public const int NumberOfCharsForDescription = 20;

        public RecipeAllViewModel()
        {
            this.Ingredients = new List<RecipeAllIngredientsViewModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }

        public int Portions { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public IList<RecipeAllIngredientsViewModel> Ingredients { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Recipe, RecipeAllViewModel>()
                .ForMember(x => x.Ingredients, opt => opt.MapFrom(c => c.RecipeIngredients));
            configuration.CreateMap<Recipe, RecipeAllViewModel>()
                .ForMember(x => x.Description, opt => opt.AddTransform(c => c.Length > NumberOfCharsForDescription ? c.Substring(0, NumberOfCharsForDescription) + "..." : c.ToString()));
        }
    }
}

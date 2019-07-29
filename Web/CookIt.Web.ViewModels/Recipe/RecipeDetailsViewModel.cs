namespace CookIt.Web.ViewModels.Recipe
{
    using System.Collections.Generic;

    using AutoMapper;
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;

    public class RecipeDetailsViewModel : IMapFrom<Recipe>, IHaveCustomMappings
    {
        public RecipeDetailsViewModel()
        {
            this.Ingreients = new List<RecipeIngredientsDetailsViewModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Portions { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public ICollection<RecipeIngredientsDetailsViewModel> Ingreients { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Recipe, RecipeDetailsViewModel>()
                .ForMember(x => x.Ingreients, opt => opt.MapFrom(c => c.RecipeIngredients));
        }
    }
}

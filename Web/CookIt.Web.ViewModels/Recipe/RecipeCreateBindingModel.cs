namespace CookIt.Web.ViewModels.Recipe
{
    using System.Collections.Generic;

    using AutoMapper;
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;
    using Microsoft.AspNetCore.Http;

    public class RecipeCreateBindingModel : IMapTo<Recipe>, IHaveCustomMappings
    {
        public RecipeCreateBindingModel()
        {
            this.Ingredients = new List<RecipeIngredientBindingModel>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Portions { get; set; }

        public string RecipeInstructions { get; set; }

        public IFormFile Image { get; set; }

        public IList<RecipeIngredientBindingModel> Ingredients { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Recipe, RecipeCreateBindingModel>()
                .ForMember(x => x.Image, c => c.Ignore());
        }
    }
}

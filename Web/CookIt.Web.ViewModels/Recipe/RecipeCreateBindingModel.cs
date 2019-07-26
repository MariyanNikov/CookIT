namespace CookIt.Web.ViewModels.Recipe
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;
    using Microsoft.AspNetCore.Http;

    public class RecipeCreateBindingModel : IMapTo<Recipe>, IHaveCustomMappings, IValidatableObject
    {
        public RecipeCreateBindingModel()
        {
            this.Ingredients = new List<RecipeIngredientBindingModel>();
        }

        // TODO: add validations
        public string Name { get; set; }

        public string Description { get; set; }

        public int Portions { get; set; }

        public string RecipeInstructions { get; set; }

        public decimal Price { get; set; }

        public IFormFile Image { get; set; }

        public IList<RecipeIngredientBindingModel> Ingredients { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Recipe, RecipeCreateBindingModel>()
                .ForMember(x => x.Image, c => c.Ignore());
            configuration.CreateMap<RecipeCreateBindingModel, Recipe>()
                .ForMember(x => x.Image, c => c.Ignore());
            configuration.CreateMap<RecipeCreateBindingModel, Recipe>()
                .ForMember(x => x.RecipeIngredients, opt => opt.MapFrom(c => c.Ingredients));
        }

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            var distinctedIngredientIds = this.Ingredients.Select(x => x.IngredientId).Distinct().ToList();

            if (distinctedIngredientIds.Count != this.Ingredients.Count)
            {
                yield return new ValidationResult("You can choose ingredient only once.");
            }
        }
    }
}

﻿namespace CookIt.Web.BindingModels.Recipe
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;
    using Microsoft.AspNetCore.Http;

    public class RecipeEditBindingModel : IMapFrom<Recipe>, IMapTo<Recipe>, IHaveCustomMappings, IValidatableObject
    {
        public RecipeEditBindingModel()
        {
            this.Ingredients = new List<RecipeEditIngredientBindingModel>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} field must be between {2} and {1} characters.", MinimumLength = 3)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "The {0} field must be between {2} and {1} characters.", MinimumLength = 10)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "The {0} field must be in the range of {1} and {2}.")]
        [Display(Name = "Portions")]
        public int Portions { get; set; }

        [Required]
        [MinLength(50, ErrorMessage = "The {0} field must be at least {1} characters.")]
        [Display(Name = "Recipe Instructions")]
        public string RecipeInstructions { get; set; }

        [Required]
        [Range(typeof(decimal), "0.0", "79228162514264337593543950335", ErrorMessage = "The {0} field must be a positive number.")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        public string Image { get; set; }

        public IFormFile ImageUpload { get; set; }

        public IList<RecipeEditIngredientBindingModel> Ingredients { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Recipe, RecipeEditBindingModel>()
                .ForMember(x => x.Ingredients, opt => opt.MapFrom(c => c.RecipeIngredients));

            configuration.CreateMap<RecipeEditBindingModel, Recipe>()
                .ForMember(x => x.RecipeIngredients, opt => opt.MapFrom(c => c.Ingredients));
        }

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            var distinctedIngredientIds = this.Ingredients.Select(x => x.IngredientId).Distinct().ToList();

            if (distinctedIngredientIds.Count != this.Ingredients.Count)
            {
                yield return new ValidationResult("You can choose specific ingredient only once.");
            }
        }
    }
}

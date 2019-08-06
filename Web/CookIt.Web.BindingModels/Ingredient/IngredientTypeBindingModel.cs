namespace CookIt.Web.BindingModels.Ingredient
{
    using System.ComponentModel.DataAnnotations;

    using CookIt.Data.Models;
    using CookIt.Services.Mapping;

    public class IngredientTypeBindingModel : IMapTo<IngredientType>
    {
        [Required]
        [StringLength(30, ErrorMessage = "{0} name must be between {2} and {1} characters.", MinimumLength = 3)]
        [Display(Name = "Ingredient Type")]
        public string Name { get; set; }
    }
}

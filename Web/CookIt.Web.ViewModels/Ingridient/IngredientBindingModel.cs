namespace CookIt.Web.ViewModels.Ingridient
{
    using System.ComponentModel.DataAnnotations;

    using CookIt.Data.Models;
    using CookIt.Services.Mapping;

    public class IngredientBindingModel : IMapTo<Ingredient>
    {
        [Required]
        [StringLength(30, ErrorMessage = "{0} must be between {2} and {1} characters.", MinimumLength = 3)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Ingredient Type")]
        public int IngredientTypeId { get; set; }
    }
}

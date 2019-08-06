namespace CookIt.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RecipeIngredient
    {
        [Required]
        public int RecipeId { get; set; }

        public Recipe Recipe { get; set; }

        [Required]
        public int IngredientId { get; set; }

        public Ingredient Ingredient { get; set; }

        [Range(1, int.MaxValue)]
        public int? Count { get; set; }

        [Range(1.0, double.MaxValue)]
        public double? Weight { get; set; }
    }
}

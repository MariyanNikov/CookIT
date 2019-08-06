namespace CookIt.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class OrderRecipe
    {
        [Required]
        public string OrderId { get; set; }

        public Order Order { get; set; }

        [Required]
        public int RecipeId { get; set; }

        public Recipe Recipe { get; set; }
    }
}

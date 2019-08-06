namespace CookIt.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CookIt.Data.Common.Models;

    public class Recipe : BaseDeletableModel<int>, IDeletableEntity
    {
        public Recipe()
        {
            this.RecipeIngredients = new HashSet<RecipeIngredient>();
            this.Reviews = new HashSet<Review>();
        }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 10)]
        public string Description { get; set; }

        [Required]
        [Range(1, 10)]
        public int Portions { get; set; }

        [Required]
        [MinLength(50)]
        public string RecipeInstructions { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        [Range(typeof(decimal), "0.0", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }

        public ICollection<OrderRecipe> OrdersRecipe { get; set; }
    }
}

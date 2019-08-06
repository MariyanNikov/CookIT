namespace CookIt.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CookIt.Data.Common.Models;

    public class Ingredient : BaseModel<int>
    {
        public Ingredient()
        {
            this.RecipeIngredients = new HashSet<RecipeIngredient>();
        }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }

        public int IngredientTypeId { get; set; }

        public IngredientType IngredientType { get; set; }

        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
    }
}

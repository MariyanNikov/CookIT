namespace CookIt.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CookIt.Data.Common.Models;

    public class IngredientType : BaseModel<int>
    {
        public IngredientType()
        {
            this.Ingredients = new HashSet<Ingredient>();
        }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }
    }
}

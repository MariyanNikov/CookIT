namespace CookIt.Data.Models
{
    using System.Collections.Generic;

    using CookIt.Data.Common.Models;

    public class Recipe : BaseDeletableModel<int>, IDeletableEntity
    {
        public Recipe()
        {
            this.RecipeIngredients = new HashSet<RecipeIngredient>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Portions { get; set; }

        public string RecipeInstructions { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
    }
}

namespace CookIt.Data.Models
{
    using System.Collections.Generic;

    using CookIt.Data.Common.Models;

    public class IngredientType : BaseModel<int>
    {
        public IngredientType()
        {
            this.Ingredients = new HashSet<Ingredient>();
        }

        public string Name { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }
    }
}

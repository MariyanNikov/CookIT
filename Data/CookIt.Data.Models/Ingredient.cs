namespace CookIt.Data.Models
{
    using CookIt.Data.Common.Models;

    public class Ingredient : BaseModel<int>
    {
        public string Name { get; set; }

        public int IngredientTypeId { get; set; }

        public IngredientType IngredientType { get; set; }
    }
}

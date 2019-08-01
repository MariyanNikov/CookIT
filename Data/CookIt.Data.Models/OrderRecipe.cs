namespace CookIt.Data.Models
{
    public class OrderRecipe
    {
        public string OrderId { get; set; }

        public Order Order { get; set; }

        public int RecipeId { get; set; }

        public Recipe Recipe { get; set; }
    }
}

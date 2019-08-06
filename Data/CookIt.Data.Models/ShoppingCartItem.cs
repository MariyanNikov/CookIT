namespace CookIt.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using CookIt.Data.Common.Models;

    public class ShoppingCartItem : BaseModel<int>
    {
        [Required]
        public int RecipeId { get; set; }

        public Recipe Recipe { get; set; }

        [Required]
        public int ShoppingCartId { get; set; }

        public ShoppingCart ShoppingCart { get; set; }
    }
}

namespace CookIt.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using CookIt.Data.Common.Models;

    public class ShoppingCartItem : BaseModel<int>
    {
        public int RecipeId { get; set; }

        public Recipe Recipe { get; set; }

        public int ShoppingCartId { get; set; }

        public ShoppingCart ShoppingCart { get; set; }
    }
}

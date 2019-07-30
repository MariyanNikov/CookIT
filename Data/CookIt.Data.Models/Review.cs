namespace CookIt.Data.Models
{
    using CookIt.Data.Common.Models;

    public class Review : BaseModel<int>
    {
        public string Content { get; set; }

        public int Stars { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public int RecipeId { get; set; }

        public Recipe Recipe { get; set; }
    }
}

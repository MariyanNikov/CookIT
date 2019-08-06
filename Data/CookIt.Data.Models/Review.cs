namespace CookIt.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using CookIt.Data.Common.Models;

    public class Review : BaseModel<int>
    {
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Content { get; set; }

        [Required]
        [Range(1, 5)]
        public int Stars { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public int RecipeId { get; set; }

        public Recipe Recipe { get; set; }
    }
}

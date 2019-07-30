namespace CookIt.Web.ViewModels.Review
{
    using System.ComponentModel.DataAnnotations;

    using CookIt.Data.Models;
    using CookIt.Services.Mapping;

    public class ReviewBindingModel : IMapTo<Review>
    {
        [Required]
        [Range(1, 5, ErrorMessage = "{0} field should be between {1} and {2}.")]
        [Display(Name = "Stars")]
        public int Stars { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "{0} field must be between {2} and {1} characters.", MinimumLength = 3)]
        [Display(Name = "Content")]
        public string Content { get; set; }
    }
}

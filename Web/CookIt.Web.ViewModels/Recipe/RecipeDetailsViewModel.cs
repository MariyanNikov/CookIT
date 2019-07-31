namespace CookIt.Web.ViewModels.Recipe
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;
    using CookIt.Web.ViewModels.Review;

    public class RecipeDetailsViewModel : IMapFrom<Recipe>, IHaveCustomMappings
    {
        public RecipeDetailsViewModel()
        {
            this.Ingredients = new List<RecipeIngredientsDetailsViewModel>();
            this.Reviews = new List<ReviewDetailsViewModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Portions { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public bool HasReviewed { get; set; }

        public double TotalReviewScore => double.IsNaN(this.Reviews.Sum(z => z.Stars) * 1.0 / this.Reviews.Count) ? 0.0 : this.Reviews.Sum(z => z.Stars) * 1.0 / this.Reviews.Count;

        public ICollection<ReviewDetailsViewModel> Reviews { get; set; }

        public ICollection<RecipeIngredientsDetailsViewModel> Ingredients { get; set; }

        public ReviewBindingModel ReviewBindingModel { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Recipe, RecipeDetailsViewModel>()
                .ForMember(x => x.Ingredients, opt => opt.MapFrom(c => c.RecipeIngredients))
                .ForMember(x => x.Reviews, opt => opt.MapFrom(c => c.Reviews.OrderByDescending(z => z.CreatedOn)));
        }
    }
}

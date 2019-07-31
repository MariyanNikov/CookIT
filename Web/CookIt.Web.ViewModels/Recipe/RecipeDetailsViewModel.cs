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
            this.Ingreients = new List<RecipeIngredientsDetailsViewModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Portions { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public bool HasReviewed { get; set; }

        public double TotalReviewScore { get; set; }

        public ICollection<ReviewDetailsViewModel> Reviews { get; set; }

        public ICollection<RecipeIngredientsDetailsViewModel> Ingreients { get; set; }

        public ReviewBindingModel ReviewBindingModel { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Recipe, RecipeDetailsViewModel>()
                .ForMember(x => x.Ingreients, opt => opt.MapFrom(c => c.RecipeIngredients))
                .ForMember(x => x.Reviews, opt => opt.MapFrom(c => c.Reviews.OrderByDescending(z => z.CreatedOn)))
                .ForMember(x => x.TotalReviewScore, opt => opt.MapFrom(c => c.Reviews.Sum(z => z.Stars) * 1.0 / c.Reviews.Count));
        }
    }
}

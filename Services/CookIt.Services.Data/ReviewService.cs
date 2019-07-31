namespace CookIt.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using CookIt.Data.Common.Repositories;
    using CookIt.Data.Models;

    public class ReviewService : IReviewService
    {
        private readonly IRepository<Review> reviewRepository;

        public ReviewService(IRepository<Review> reviewRepository)
        {
            this.reviewRepository = reviewRepository;
        }

        public async Task<bool> AddAsync<TModel>(TModel review, int recipeId, string userId)
        {
            var reviewForDb = Mapper.Map<Review>(review);
            reviewForDb.RecipeId = recipeId;
            reviewForDb.ApplicationUserId = userId;

            await this.reviewRepository.AddAsync(reviewForDb);
            await this.reviewRepository.SaveChangesAsync();
            return true;
        }

        public bool HasReviewedRecipeByUserId(int recipeId, string userId)
        {
            var hasReviewed = this.reviewRepository.All().Any(x => x.ApplicationUserId == userId && x.RecipeId == recipeId);

            return hasReviewed;
        }
    }
}

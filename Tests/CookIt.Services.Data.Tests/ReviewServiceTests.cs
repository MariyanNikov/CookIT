namespace CookIt.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookIt.Data.Common.Repositories;
    using CookIt.Data.Models;
    using CookIt.Web.ViewModels.Review;
    using Moq;

    using Xunit;

    public class ReviewServiceTests
    {
        public ReviewServiceTests()
        {
            MapperInitialize.Initialize();
        }

        [Fact]
        public async Task ReviewAddAsyncShouldAddReviewProperly()
        {
            var reviewRepository = new Mock<IRepository<Review>>();
            var service = new ReviewService(reviewRepository.Object);

            var actualResult = await service.AddAsync<ReviewBindingModel>(this.DummyDataReviewModel(), 1, "1");

            Assert.True(actualResult == true, "Review AddAsync should return true upon successfull add of review.");
            reviewRepository.Verify(x => x.AddAsync(It.IsAny<Review>()), Times.Once, "AddAsync method of Review Repository should be called once.");
            reviewRepository.Verify(x => x.SaveChangesAsync(), Times.Once, "SaveChangesAsync method of Review Repository should be called once.");
        }

        [Theory]
        [InlineData(1, "1", true)]
        [InlineData(1, "2", false)]
        [InlineData(2, "1", false)]
        public void HasReviewedRecipeByUserIdShouldWorkProperly(int recipeId, string userId, bool expectedResult)
        {
            var reviewRepository = new Mock<IRepository<Review>>();
            reviewRepository.Setup(x => x.All()).Returns(this.DummyDataReviews().AsQueryable());
            var service = new ReviewService(reviewRepository.Object);

            var actualResult = service.HasReviewedRecipeByUserId(recipeId, userId);

            Assert.True(actualResult == expectedResult, $"Has Reviewd Recipe should return {expectedResult} with recipeId: {recipeId} and userId {userId}");
        }

        private List<Review> DummyDataReviews()
        {
            var reviews = new List<Review>
            {
                new Review
                {
                    Id = 1,
                    ApplicationUserId = "1",
                    RecipeId = 1,
                    Content = "First Review ContentFirst Review ContentFirst Review Content",
                    Stars = 1,
                },
                new Review
                {
                    Id = 2,
                    ApplicationUserId = "2",
                    RecipeId = 2,
                    Content = "Second Review ContentSecond Review ContentSecond Review Content",
                    Stars = 2,
                },
                new Review
                {
                    Id = 3,
                    ApplicationUserId = "3",
                    RecipeId = 3,
                    Content = "Third Review ContentThird Review ContentThird Review Content",
                    Stars = 3,
                },
            };

            return reviews;
        }

        private ReviewBindingModel DummyDataReviewModel()
        {
            var reviewModel = new ReviewBindingModel
            {
                Content = "Review ContentReview ContentReview ContentReview ContentReview Content",
                Stars = 5,
            };
            return reviewModel;
        }
    }
}

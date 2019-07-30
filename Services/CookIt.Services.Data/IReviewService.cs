namespace CookIt.Services.Data
{
    using System.Threading.Tasks;

    public interface IReviewService
    {
        Task<bool> AddAsync<TModel>(TModel review, int recipeId, string userId);

        bool HasReviewedRecipeByUserId(int recipeId, string userId);
    }
}

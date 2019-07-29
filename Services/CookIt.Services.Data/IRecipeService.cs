namespace CookIt.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IRecipeService
    {
        Task<bool> CreateRecipe<TModel>(TModel recipe, string imageUrl);

        bool CheckRecipeByName(string name);

        IQueryable<TModel> GetAllRecipesWithDeleted<TModel>();

        bool CheckRecipeById(int id);

        Task<bool> SoftDeleteRecipe(int id);

        Task<bool> UnDeleteRecipe(int id);

        TModel FindRecipeById<TModel>(int id);

        IQueryable<TModel> GetAllRecipeIngredients<TModel>();

        Task<bool> UpdateRecipe<TModel>(TModel recipe, int recipeId);

        IQueryable<TModel> GetAllRecipesWithoutDeleted<TModel>();

        TModel GetRecipeWithoutDeleted<TModel>(int id);
    }
}

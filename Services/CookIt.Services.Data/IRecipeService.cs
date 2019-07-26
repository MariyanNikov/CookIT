namespace CookIt.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IRecipeService
    {
        Task<bool> CreateRecipe<TModel>(TModel recipe, string imageUrl);

        bool CheckRecipeByName(string name);

        IQueryable<TModel> GetAllRecipes<TModel>();
    }
}

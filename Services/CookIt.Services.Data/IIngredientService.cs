namespace CookIt.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IIngredientService
    {
        // TODO: Naming
        Task<bool> CreateIngredientType(string name);

        bool CheckIfIngredientTypeExistByName(string name);

        bool CheckIfIngredientTypeExistById(int id);

        Task<bool> RemoveIngredientTypeById(int id);

        string GetIngredientTypeNameById(int id);

        IQueryable<TModel> GetAllIngreientTypes<TModel>();

        Task<bool> CreateIngredient<TModel>(TModel ingredientModel);

        IQueryable<TModel> GetAllIngreients<TModel>();

        string GetIngredientNameById(int id);

        bool IngredientExistsByName(string name);

        Task<bool> RemoveIngredientById(int id);

        Task<bool> CheckExistingIngredientId(ICollection<int> ingredientIds);
    }
}

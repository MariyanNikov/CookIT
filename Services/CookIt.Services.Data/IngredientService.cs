namespace CookIt.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using CookIt.Data.Common.Repositories;
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class IngredientService : IIngredientService
    {
        private readonly IRepository<IngredientType> ingredientTypeRepository;
        private readonly IRepository<Ingredient> ingredientRepository;

        public IngredientService(IRepository<IngredientType> ingredientTypeRepository, IRepository<Ingredient> ingredientRepository)
        {
            this.ingredientTypeRepository = ingredientTypeRepository;
            this.ingredientRepository = ingredientRepository;
        }

        public async Task<bool> CreateIngredientType(string name)
        {
            var ingredientType = new IngredientType { Name = name };

            await this.ingredientTypeRepository.AddAsync(ingredientType);
            await this.ingredientTypeRepository.SaveChangesAsync();
            return true;
        }

        public bool CheckIfIngredientTypeExistByName(string name)
        {
            var ingredientFromDb = this.ingredientTypeRepository.All().SingleOrDefault(x => x.Name == name);
            if (ingredientFromDb != null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> RemoveIngredientTypeById(int id)
        {
            var ingredientFromDb = this.ingredientTypeRepository.All().SingleOrDefault(x => x.Id == id);

            if (ingredientFromDb == null)
            {
                return false;
            }

            this.ingredientTypeRepository.Delete(ingredientFromDb);
            await this.ingredientTypeRepository.SaveChangesAsync();

            return true;
        }

        public string GetIngredientTypeNameById(int id)
        {
            var ingredientFromDb = this.ingredientTypeRepository.All().SingleOrDefault(x => x.Id == id);

            return ingredientFromDb?.Name;
        }

        public IQueryable<TModel> GetAllIngreientTypes<TModel>()
        {
            return this.ingredientTypeRepository.All().To<TModel>();
        }

        // TODO: Naming
        public bool CheckIfIngredientTypeExistById(int id)
        {
            var ingredientFromDb = this.ingredientTypeRepository.All().SingleOrDefault(x => x.Id == id);
            if (ingredientFromDb != null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> CreateIngredient<TModel>(TModel ingredientModel)
        {
            var ingredient = Mapper.Map<Ingredient>(ingredientModel);

            await this.ingredientRepository.AddAsync(ingredient);
            await this.ingredientRepository.SaveChangesAsync();

            return true;
        }

        public IQueryable<TModel> GetAllIngreients<TModel>()
        {
            var ingredients = this.ingredientRepository.All().Include(x => x.IngredientType).To<TModel>();
            return ingredients;
        }

        public string GetIngredientNameById(int id)
        {
            var ingredientFromDb = this.ingredientRepository.All().SingleOrDefault(x => x.Id == id);

            return ingredientFromDb?.Name;
        }

        public bool IngredientExistsByName(string name)
        {
            var ingredientFromDb = this.ingredientRepository.All().SingleOrDefault(x => x.Name == name);
            if (ingredientFromDb != null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> RemoveIngredientById(int id)
        {
            var ingredientFromDb = this.ingredientRepository.All().SingleOrDefault(x => x.Id == id);

            if (ingredientFromDb == null)
            {
                return false;
            }

            this.ingredientRepository.Delete(ingredientFromDb);
            await this.ingredientTypeRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CheckExistingIngredientId(ICollection<int> ingredientIds)
        {
            var ingredients = await this.ingredientRepository.All().Where(x => ingredientIds.Any(id => x.Id == id)).ToListAsync();
            if (ingredientIds.Count == ingredients.Count)
            {
                return true;
            }

            return false;
        }

        public bool HasIngredientWithType(int ingredientTypeId)
        {
            return this.ingredientTypeRepository.All().Include(x => x.Ingredients).SingleOrDefault(x => x.Id == ingredientTypeId).Ingredients.Any();
        }

        public bool HasRecipeWithIngredient(int ingredientId)
        {
            return this.ingredientRepository.All().Include(x => x.RecipeIngredients).SingleOrDefault(x => x.Id == ingredientId).RecipeIngredients.Any();
        }
    }
}

namespace CookIt.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using CookIt.Data.Common.Repositories;
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;

    public class RecipeService : IRecipeService
    {
        private readonly IDeletableEntityRepository<Recipe> recipeRepository;

        public RecipeService(IDeletableEntityRepository<Recipe> recipeRepository)
        {
            this.recipeRepository = recipeRepository;
        }

        public async Task<bool> CreateRecipe<TModel>(TModel recipe, string imageUrl)
        {
            var recipeForDb = Mapper.Map<Recipe>(recipe);
            recipeForDb.Image = imageUrl;

            await this.recipeRepository.AddAsync(recipeForDb);
            await this.recipeRepository.SaveChangesAsync();

            return true;
        }

        public bool CheckRecipeByName(string name)
        {
            var recipeExistsByName = this.recipeRepository.AllWithDeleted().Any(x => x.Name == name);

            return recipeExistsByName;
        }

        public IQueryable<TModel> GetAllRecipesWithDeleted<TModel>()
        {
            return this.recipeRepository.AllWithDeleted().To<TModel>();
        }

        public bool CheckRecipeById(int id)
        {
            bool recipeExistsById = this.recipeRepository.AllWithDeleted().Any(x => x.Id == id);

            return recipeExistsById;
        }

        public async Task<bool> SoftDeleteRecipe(int id)
        {
            var recipe = this.recipeRepository.All().SingleOrDefault(x => x.Id == id);

            this.recipeRepository.Delete(recipe);
            await this.recipeRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnDeleteRecipe(int id)
        {
            var recipe = this.recipeRepository.AllWithDeleted().SingleOrDefault(x => x.Id == id);

            this.recipeRepository.Undelete(recipe);
            await this.recipeRepository.SaveChangesAsync();
            return true;
        }
    }
}

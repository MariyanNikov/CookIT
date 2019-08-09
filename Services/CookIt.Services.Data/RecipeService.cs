namespace CookIt.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using CookIt.Data.Common.Repositories;
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class RecipeService : IRecipeService
    {
        private readonly IDeletableEntityRepository<Recipe> recipeRepository;
        private readonly IRepository<RecipeIngredient> recipeIngredientRepository;

        public RecipeService(IDeletableEntityRepository<Recipe> recipeRepository, IRepository<RecipeIngredient> recipeIngredientRepository)
        {
            this.recipeRepository = recipeRepository;
            this.recipeIngredientRepository = recipeIngredientRepository;
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

        public TModel FindRecipeById<TModel>(int id)
        {
            var recipe = this.recipeRepository.AllWithDeleted().Where(x => x.Id == id).To<TModel>();
            return recipe.SingleOrDefault();
        }

        public async Task<bool> UpdateRecipe<TModel>(TModel recipe, int recipeId)
        {
            var recipeIngredients = this.recipeIngredientRepository.AllAsNoTracking().Where(x => x.RecipeId == recipeId).ToList();
            this.recipeIngredientRepository.DeleteAll(recipeIngredients);
            await this.recipeIngredientRepository.SaveChangesAsync();

            var recipeForDb = Mapper.Map<Recipe>(recipe);
            this.recipeRepository.Update(recipeForDb);
            await this.recipeRepository.SaveChangesAsync();

            return true;
        }

        public IQueryable<TModel> GetAllRecipesWithoutDeleted<TModel>()
        {
            var recipes = this.recipeRepository.All().To<TModel>();

            return recipes;
        }

        public TModel GetRecipeWithoutDeleted<TModel>(int id)
        {
            var recipe = this.recipeRepository.All().Include(x => x.Reviews).Where(x => x.Id == id).To<TModel>().FirstOrDefault();

            return recipe;
        }

        public IQueryable<TModel> GetHighestRatingsRecipes<TModel>()
        {
            var recipes = this.recipeRepository.All().Where(x => x.Reviews.Any()).OrderByDescending(c => (c.Reviews.Sum(z => z.Stars) * 1.0) / c.Reviews.Count).To<TModel>();

            return recipes;
        }

        public IQueryable<TModel> GetLatestRecipes<TModel>()
        {
            var recipes = this.recipeRepository.All().OrderByDescending(x => x.Id).To<TModel>();

            return recipes;
        }
    }
}

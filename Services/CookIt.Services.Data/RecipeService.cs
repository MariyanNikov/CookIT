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
        private readonly IRepository<Recipe> recipeRepository;

        public RecipeService(IRepository<Recipe> recipeRepository)
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
            var recipe = this.recipeRepository.All().SingleOrDefault(x => x.Name == name);

            if (recipe != null)
            {
                return true;
            }

            return false;
        }

        public IQueryable<TModel> GetAllRecipes<TModel>()
        {
            return this.recipeRepository.All().To<TModel>();
        }
    }
}

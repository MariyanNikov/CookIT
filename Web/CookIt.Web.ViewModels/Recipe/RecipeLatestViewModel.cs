namespace CookIt.Web.ViewModels.Recipe
{
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;

    public class RecipeLatestViewModel : IMapFrom<Recipe>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }
    }
}

namespace CookIt.Web.ViewModels.Ingridient
{
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;

    public class AllIngredientViewModel : IMapFrom<Ingredient>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string IngredientTypeName { get; set; }
    }
}

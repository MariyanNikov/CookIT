namespace CookIt.Web.ViewModels.Ingridient
{
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;

    public class AllIngredientTypeViewModel : IMapFrom<IngredientType>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}

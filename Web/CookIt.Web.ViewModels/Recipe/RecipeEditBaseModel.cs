namespace CookIt.Web.ViewModels.Recipe
{
    using System.Collections.Generic;

    using CookIt.Web.ViewModels.Ingridient;

    public class RecipeEditBaseModel
    {
        public RecipeEditBaseModel()
        {
            this.Ingredients = new List<AllIngredientViewModel>();
        }

        public IList<AllIngredientViewModel> Ingredients { get; set; }

        public RecipeEditViewModel InputModel { get; set; }

        public int NumberOfIngredients { get; set; } = 1;

        public int Id { get; set; }
    }
}

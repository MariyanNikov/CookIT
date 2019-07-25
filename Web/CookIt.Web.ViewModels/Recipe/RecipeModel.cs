namespace CookIt.Web.ViewModels.Recipe
{
    using System.Collections.Generic;

    using CookIt.Web.ViewModels.Ingridient;

    public class RecipeModel
    {
        public RecipeModel()
        {
            this.Ingredients = new List<AllIngredientViewModel>();
        }

        public IList<AllIngredientViewModel> Ingredients { get; set; }

        public RecipeCreateBindingModel InputModel { get; set; }

        public int NumberOfIngredients { get; set; } = 1;
    }
}

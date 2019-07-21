namespace CookIt.Web.Infrastructure.ViewComponents
{
    using System.Threading.Tasks;

    using CookIt.Services.Data;
    using CookIt.Web.ViewModels.Ingridient;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class IngredientTypesViewComponent : ViewComponent
    {
        private readonly IIngredientService ingredientService;

        public IngredientTypesViewComponent(IIngredientService ingredientService)
        {
            this.ingredientService = ingredientService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var ingredientTypes = await this.ingredientService.GetAllIngreientTypes<AllIngredientTypeViewModel>().ToListAsync();
            return this.View(ingredientTypes);
        }
    }
}

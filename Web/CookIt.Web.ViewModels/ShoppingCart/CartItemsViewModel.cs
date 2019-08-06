namespace CookIt.Web.ViewModels.ShoppingCart
{
    using System.Linq;

    using AutoMapper;
    using CookIt.Data.Models;
    using CookIt.Services.Mapping;

    public class CartItemsViewModel : IMapFrom<ShoppingCartItem>, IHaveCustomMappings
    {
        private const string ListofIngredientsSeparator = ", ";

        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public int Portions { get; set; }

        public string ListOfIngredients { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ShoppingCartItem, CartItemsViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Recipe.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Recipe.Name))
                .ForMember(x => x.Image, opt => opt.MapFrom(c => c.Recipe.Image))
                .ForMember(x => x.Price, opt => opt.MapFrom(c => c.Recipe.Price))
                .ForMember(x => x.Portions, opt => opt.MapFrom(c => c.Recipe.Portions))
                .ForMember(x => x.ListOfIngredients, opt => opt.MapFrom(c => string.Join(ListofIngredientsSeparator, c.Recipe.RecipeIngredients.Select(z => z.Ingredient.Name))));
        }
    }
}

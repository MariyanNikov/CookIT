namespace CookIt.Services.Data.Tests
{
    using System.Reflection;

    using CookIt.Services.Mapping;
    using CookIt.Web.BindingModels.Address;
    using CookIt.Web.ViewModels;

    public static class MapperInitialize
    {
        public static void Initialize()
        {
            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).GetTypeInfo().Assembly,
                typeof(AddressBindingModel).GetTypeInfo().Assembly);
        }
    }
}

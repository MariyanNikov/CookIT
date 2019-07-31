namespace CookIt.Data.Extensions
{
    using System.Security.Claims;
    using System.Security.Principal;

    public static class IdentityExtentions
    {
        public static string GetFullName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("FullName");

            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}

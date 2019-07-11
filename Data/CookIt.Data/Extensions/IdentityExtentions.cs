namespace CookIt.Data.Extensions
{

    using System.Security.Claims;
    using System.Security.Principal;

    using CookIt.Data.Models;
    using Microsoft.AspNetCore.Identity;

    public static class IdentityExtentions
    {

        public static string GetFullName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("FullName");

            return (claim != null) ? claim.Value : string.Empty;
        }

    }
}

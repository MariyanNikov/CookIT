namespace CookIt.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookIt.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class AdminSeeder : ISeeder
    {
        private const string RootName = "root";

        private const string RootPassword = "root123";

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedRootAdminAsync(userManager, RootName, RootPassword);
        }

        private static async Task SeedRootAdminAsync(UserManager<ApplicationUser> userManager, string adminName, string password)
        {

            var user = userManager.FindByEmailAsync($"{adminName}@{adminName}.com");
            if (user == null)
            {
                var root = new ApplicationUser
                {
                    Email = $"{adminName}@{adminName}.com",
                    EmailConfirmed = true,
                    FirstName = $"{adminName}",
                    LastName = $"{adminName}",
                    PhoneNumber = "0889123456",
                    UserName = $"{adminName}@{adminName}.com",
                };
                root.Claims.Add(new IdentityUserClaim<string> { ClaimType = "FullName", ClaimValue = $"{adminName}" });

                var result = await userManager.CreateAsync(root, password);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }

                var addToRoleResult = await userManager.AddToRoleAsync(root, "Administrator");

                if (!addToRoleResult.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }

        }
    }
}

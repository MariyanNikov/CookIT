namespace CookIt.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CookIt.Common;
    using CookIt.Data.Models;
    using Microsoft.AspNetCore.Identity;

    public class AdminSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Users.Any())
            {
                return;
            }

            PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = "root@root.com",
                NormalizedEmail = "root@root.com".ToUpper(),
                FirstName = "root",
                LastName = "root",
                PhoneNumber = "0123456789",
                UserName = "root@root.com",
                NormalizedUserName = "root@root.com".ToUpper(),
            };

            user.Claims.Add(new IdentityUserClaim<string> { ClaimType = "FullName", ClaimValue = $"{user.FirstName}" });
            user.SecurityStamp = Guid.NewGuid().ToString();
            var password = hasher.HashPassword(user, "123456");
            user.PasswordHash = password;
            var role = dbContext.Roles.SingleOrDefault(x => x.Name == GlobalConstants.AdministratorRoleName);
            user.Roles.Add(new IdentityUserRole<string> { RoleId = role.Id });
            await dbContext.Users.AddAsync(user);
        }
    }
}

using Microsoft.AspNetCore.Identity;
using San3a.Core.Entities;
using San3a.Core.Enums;

namespace San3a.WebApi
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Enum.GetNames(typeof(UserType)))
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            string superAdminEmail = "superadmin@system.com";
            var superAdmin = await userManager.FindByEmailAsync(superAdminEmail);

            if (superAdmin == null)
            {
                var user = new AppUser
                {
                    UserName = "SuperAdmin",
                    Email = superAdminEmail,
                    EmailConfirmed = true,
                    NationalId = "00000000000000",
                    FullName = "Super Admin",
                    City = "Mansoura",
                    CreatedAt = DateTime.UtcNow,
                };

                var result = await userManager.CreateAsync(user, "Super@123");

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, "SuperAdmin");
            }
        }
    }
}
using Microsoft.AspNetCore.Identity;
using San3a.Core.Entities;
using San3a.Infrastructure.Data;

namespace San3a.WebApi.Data
{
    public static class DataSeeder
    {
        #region Public Methods
        public static async Task SeedSuperAdminAsync(UserManager<AppUser> userManager, AppDbContext context)
        {
            var superAdminEmail = "superadmin@san3a.com";
            
            var existingAdmin = await userManager.FindByEmailAsync(superAdminEmail);
            if (existingAdmin != null)
            {
                Console.WriteLine("Super Admin already exists.");
                return;
            }

            var superAdminUser = new AppUser
            {
                UserName = "superadmin",
                Email = superAdminEmail,
                EmailConfirmed = true,
                FullName = "Super Admin",
                Address = "Admin Office",
                City = "Cairo",
                PhoneNumber = "01000000000"
            };

            var result = await userManager.CreateAsync(superAdminUser, "SuperAdmin@123");
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                
                var admin = new Admin
                {
                    Id = superAdminUser.Id,
                    IsSuperAdmin = true
                };

                context.Admins.Add(admin);
                await context.SaveChangesAsync();
                
                Console.WriteLine("Super Admin created successfully!");
                Console.WriteLine($"Email: {superAdminEmail}");
                Console.WriteLine("Password: SuperAdmin@123");
            }
            else
            {
                Console.WriteLine("Failed to create Super Admin:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error.Description}");
                }
            }
        }
        #endregion
    }
}

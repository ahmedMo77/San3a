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
                Governorate = "Cairo",
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
       
        public static async Task SeedServicesAsync(AppDbContext context)
        {
            // Check if services already exist
            if (context.Services.Any())
            {

                Console.WriteLine("Services already seeded.");
                return;
            }

            var services = new List<Service>
            {
                new Service
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "بلاط",
                    Description = "أعمال تركيب وصيانة البلاط والسيراميك والرخام",
                    CreatedAt = DateTime.UtcNow
                },
                new Service
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "نجار",
                    Description = "أعمال النجارة والأثاث والديكورات الخشبية",
                    CreatedAt = DateTime.UtcNow
                },
                new Service
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "دهانات",
                    Description = "أعمال الدهان والطلاء والديكور",
                    CreatedAt = DateTime.UtcNow
                },
                new Service
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "سباكة",
                    Description = "أعمال السباكة وإصلاح المواسير والصرف الصحي",
                    CreatedAt = DateTime.UtcNow
                },
                new Service
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "كهرباء",
                    Description = "أعمال الكهرباء والصيانة الكهربائية والإضاءة",
                    CreatedAt = DateTime.UtcNow
                },
                new Service
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "تكييف",
                    Description = "صيانة وتركيب وإصلاح أجهزة التكييف والتبريد",
                    CreatedAt = DateTime.UtcNow
                },
                new Service
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "تنظيف",
                    Description = "خدمات التنظيف المنزلي والتجاري والشامل",
                    CreatedAt = DateTime.UtcNow
                },
                new Service
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "بناء",
                    Description = "أعمال البناء والمحارة والإنشاءات",
                    CreatedAt = DateTime.UtcNow
                },
                new Service
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "منجد",
                    Description = "تنجيد وتلبيس الأثاث والكنب والمفروشات",
                    CreatedAt = DateTime.UtcNow
                },
                new Service
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "كرسانة",
                    Description = "خدمات نقل الأثاث والبضائع والتخزين",
                    CreatedAt = DateTime.UtcNow
                },
                new Service
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "عزل وسقف",
                    Description = "أعمال العزل الحراري والمائي وإصلاح الأسقف",
                    CreatedAt = DateTime.UtcNow
                },
                new Service
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "زراعة و تنسيق الأشجار",
                    Description = "خدمات الزراعة وتنسيق الحدائق والعناية بالنباتات",
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Services.AddRangeAsync(services);
            await context.SaveChangesAsync();

            Console.WriteLine($"Successfully seeded {services.Count} services:");
            foreach (var service in services)
            {
                Console.WriteLine($"- {service.Name} (ID: {service.Id})");
            }
        }

        #endregion
    }
}
       


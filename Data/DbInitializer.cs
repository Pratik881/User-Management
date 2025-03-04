using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Data;

public class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        // Apply migrations instead of EnsureCreated()
         context.Database.Migrate();

        var roleNames = new[] { "Admin", "User" };

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        
        var adminUser = await userManager.FindByNameAsync("admin");

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                FullName = "Pratik admin",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(adminUser, "SecurePassword123!"); 

            if (result.Succeeded)
            {
                
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}

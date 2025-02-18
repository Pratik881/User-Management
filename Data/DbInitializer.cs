using Microsoft.AspNetCore.Identity;
using UserManagementSystem.Data;

public class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated(); // This ensures that the database is created (you can skip this if you are using migrations)

        // Check if roles already exist
        var roleNames = new[] { "Admin", "User" };

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Check if the admin user exists
        var adminUser = await userManager.FindByNameAsync("admin");

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@example.com",
                FullName = "Pratik admin",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(adminUser, "SecurePassword123!"); // Use a secure password

            if (result.Succeeded)
            {
                // Assign the Admin role to the created user
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}

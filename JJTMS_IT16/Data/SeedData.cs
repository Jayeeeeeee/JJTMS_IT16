using JJTMS_IT16.Models;
using Microsoft.AspNetCore.Identity;

namespace JJTMS_IT16.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Ensure roles are created
            string[] roleNames = { "Admin", "Team Leader", "Team Member" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Define admin user details
            string adminEmail = "admin@jjtms.com";
            string adminPassword = "Admin123!";

            // Check if the admin user already exists
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                // Create the admin user
                var newAdminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var createAdmin = await userManager.CreateAsync(newAdminUser, adminPassword);
                if (createAdmin.Succeeded)
                {
                    // Assign Admin role to the user
                    await userManager.AddToRoleAsync(newAdminUser, "Admin");
                }
            }
            else
            {
                // Reset password if user already exists
                var token = await userManager.GeneratePasswordResetTokenAsync(adminUser);
                await userManager.ResetPasswordAsync(adminUser, token, adminPassword);
            }
        }
    }
}

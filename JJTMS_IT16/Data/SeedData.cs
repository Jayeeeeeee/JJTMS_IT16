using JJTMS_IT16.Models;
using Microsoft.AspNetCore.Identity;

namespace JJTMS_IT16.Data
{
    public static class SeedData
    {
        public static async Task Initialize(RoleManager<IdentityRole> roleManager, UserManager<UserModel> userManager)
        {
            string[] roleNames = { "Admin", "Team Leader", "Team Member" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Create the roles
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create a default Admin user if it doesn't exist
            var adminEmail = "admin@jjtms.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new UserModel
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                string adminPassword = "Admin123!"; // Change to a secure password
                var createAdminUser = await userManager.CreateAsync(user, adminPassword);
                if (createAdminUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}

using Microsoft.AspNetCore.Identity;

namespace BookstoreWebApp.Seed
{
    public class IdentitySeeder
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> rolemanager)
        {
            string[] roles = { "Admin", "Client" };
            foreach (var role in roles)
            {
                if (!await rolemanager.RoleExistsAsync(role))
                {
                    await rolemanager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        
    }
}
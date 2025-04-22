using LAHJAAPI.Models;
using Microsoft.AspNetCore.Identity;
using LAHJAAPI.Utilities;

namespace LAHJAAPI.Utilities.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(IServiceScope scope)
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (await roleManager.RoleExistsAsync(UserRole.Admin))
            {
                return;
            }
            await roleManager.CreateAsync(new IdentityRole { Name = UserRole.Admin });
            await roleManager.CreateAsync(new IdentityRole { Name = UserRole.SuperVisor });
            await roleManager.CreateAsync(new IdentityRole { Name = UserRole.User });
        }



    }
}
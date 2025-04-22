using LAHJAAPI.Models;
using Microsoft.AspNetCore.Identity;
using LAHJAAPI.Utilities;

namespace LAHJAAPI.Utilities.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedAdminAsync(IServiceScope scope)
        {
            var userStore = scope.ServiceProvider.GetRequiredService<IUserStore<ApplicationUser>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var emailStore = (IUserEmailStore<ApplicationUser>)userStore;
            var PhoneNumberStore = (IUserPhoneNumberStore<ApplicationUser>)userStore;
            var ClaimStore = (IUserClaimStore<ApplicationUser>)userStore;

            string email = "admin@gmail.com";
            string phoneNumber = "712360562";
            string firstName = "Admin";
            string lastName = "Asg";

            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new ApplicationUser();
                await userStore.SetUserNameAsync(user, email, CancellationToken.None);
                await emailStore.SetEmailAsync(user, email, CancellationToken.None);
                await PhoneNumberStore.SetPhoneNumberAsync(user, phoneNumber, CancellationToken.None);
                user.EmailConfirmed = true;
                user.FirstName = firstName;
                user.LastName = lastName;
                //await ClaimStore.AddClaimsAsync(user, [
                //    new Claim(ClaimTypes2.FirstName, firstName),
                //new Claim(ClaimTypes2.LastName, lastName),
                //], CancellationToken.None);

                await userManager.CreateAsync(user, "Admin123*");
                await userManager.AddToRoleAsync(user, UserRole.Admin);
            }
            //await roleManager.SeedClaimsForAdmin();
        }

        //private async static Task SeedClaimsForAdmin(this RoleManager<IdentityRole> roleManager)
        //{
        //    var adminRole = await roleManager.FindByNameAsync("admin");
        //    await roleManager.AddPermissionClaim(adminRole, "Department");
        //    await roleManager.AddPermissionClaim(adminRole, "Material");
        //    await roleManager.AddPermissionClaim(adminRole, "Post");
        //    await roleManager.AddPermissionClaim(adminRole, "Question");
        //    await roleManager.AddPermissionClaim(adminRole, "Comment");
        //    await roleManager.AddPermissionClaim(adminRole, "Answer");
        //}


        //public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        //{
        //    var allClaims = await roleManager.GetClaimsAsync(role);
        //    var allPermissions = new List<string>()
        //    {
        //        "create","view"
        //    };
        //    foreach (var permission in allPermissions)
        //    {
        //        if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
        //        {
        //            await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
        //        }
        //    }
        //}
    }
}
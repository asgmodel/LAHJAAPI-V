using LAHJAAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LAHJAAPI.CustomPolicy
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public PermissionHandler(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var user = context.User;

            if (user == null || !user.Identity.IsAuthenticated)
                return;

            var roles = await _userManager.GetRolesAsync(await _userManager.GetUserAsync(user));

            foreach (var role in roles)
            {
                var IdentityRole = await _roleManager.FindByNameAsync(role);
                if (IdentityRole == null) continue;

                var claims = await _roleManager.GetClaimsAsync(IdentityRole);
                if (claims.Any(c => c.Type == requirement.ClaimType && c.Value == requirement.ClaimValue))
                {
                    context.Succeed(requirement);
                    return;
                }
            }
        }
    }

}

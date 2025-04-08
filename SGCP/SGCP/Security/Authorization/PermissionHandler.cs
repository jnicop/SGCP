using Microsoft.AspNetCore.Authorization;

namespace SGCP.Security.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var permissionClaim = context.User.FindFirst("permissions");

            if (permissionClaim != null)
            {
                var permissions = permissionClaim.Value.Split(',');

                if (permissions.Contains(requirement.Permission))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}

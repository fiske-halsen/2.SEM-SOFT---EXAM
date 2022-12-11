
using Microsoft.AspNetCore.Authorization;

namespace GraphqlDemo.Authorization
{
    public class HasRoleTypeHandler : AuthorizationHandler<RoleTypeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            RoleTypeRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "RoleId" && c.Issuer == requirement.Issuer))
            {
                return Task.CompletedTask;
            }

            var roleId = context.User.FindAll(c => c.Type == "RoleId" && c.Issuer == requirement.Issuer).ToList()[0]
                .Value;

            if (requirement.RoleId.ToString() == roleId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
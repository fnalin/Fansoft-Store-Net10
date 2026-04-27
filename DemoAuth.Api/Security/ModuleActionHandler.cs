using Microsoft.AspNetCore.Authorization;

namespace DemoAuth.Api.Security;

public sealed class ModuleActionHandler : AuthorizationHandler<ModuleActionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ModuleActionRequirement requirement)
    {
        if (context.User.HasClaim("god_mode", "true"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var expectedPermission = $"{requirement.Module}:{requirement.Action}";

        if (context.User.HasClaim("permission", expectedPermission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
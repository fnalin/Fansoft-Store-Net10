using System.Security.Claims;
using DemoAuth.Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace DemoAuth.Api.Security;

public sealed class ModuleAccessHandler : AuthorizationHandler<ModuleAccessRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ModuleAccessRequirement requirement)
    {
        var user = context.User;

        // GOD MODE
        if (user.HasClaim("god_mode", "true"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var permissions = user.FindAll("permission");

        foreach (var claim in permissions)
        {
            var parts = claim.Value.Split(':');

            if (parts.Length != 2)
                continue;

            var module = parts[0];

            if (!int.TryParse(parts[1], out var level))
                continue;

            if (module == requirement.Module &&
                level >= (int)requirement.MinimumLevel)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
        }

        return Task.CompletedTask;
    }
}
using System.Security.Claims;
using DemoAuth.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace DemoAuth.Api.Security;

public sealed class ModuleAccessHandler : AuthorizationHandler<ModuleAccessRequirement>
{
    private readonly AppDbContext _dbContext;

    public ModuleAccessHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ModuleAccessRequirement requirement)
    {
        var userName =
            context.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? context.User.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(userName))
            return;

        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserName == userName);

        if (user is null)
            return;

        if (user.IsTi)
        {
            context.Succeed(requirement);
            return;
        }

        var permission = await _dbContext.UserPermissions
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.UserName == userName &&
                x.Module == requirement.Module);

        if (permission is null)
            return;

        if (permission.Level >= requirement.MinimumLevel)
        {
            context.Succeed(requirement);
        }
    }
}
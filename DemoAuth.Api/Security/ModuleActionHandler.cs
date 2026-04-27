using System.Security.Claims;
using DemoAuth.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace DemoAuth.Api.Security;

public sealed class ModuleActionHandler : AuthorizationHandler<ModuleActionRequirement>
{
    private readonly AppDbContext _dbContext;

    public ModuleActionHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        ModuleActionRequirement requirement)
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

        if (requirement.Action == "Atendimento")
        {
            var canHandleTickets = await _dbContext.UserPermissions
                .AsNoTracking()
                .AnyAsync(x =>
                    x.UserName == userName &&
                    x.Module == requirement.Module &&
                    x.CanHandleTickets);

            if (canHandleTickets)
            {
                context.Succeed(requirement);
            }
        }
    }
}
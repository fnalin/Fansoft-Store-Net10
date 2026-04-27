using Microsoft.AspNetCore.Authorization;

namespace DemoAuth.Api.Security;

public sealed class ModuleActionRequirement : IAuthorizationRequirement
{
    public ModuleActionRequirement(string module, string action)
    {
        Module = module;
        Action = action;
    }

    public string Module { get; }

    public string Action { get; }
}

using System;
using DemoAuth.Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace DemoAuth.Api.Security;

public sealed class ModuleAccessRequirement : IAuthorizationRequirement
{
    public ModuleAccessRequirement(string module, AccessLevel minimumLevel)
    {
        Module = module;
        MinimumLevel = minimumLevel;
    }

    public string Module { get; }

    public AccessLevel MinimumLevel { get; }
}

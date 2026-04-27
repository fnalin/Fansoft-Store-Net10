using System;

namespace DemoAuth.Api.Models;

public sealed class UserPermission
{
    public int Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Module { get; set; } = string.Empty;

    public AccessLevel Level { get; set; }

    public bool CanHandleTickets { get; set; }
}

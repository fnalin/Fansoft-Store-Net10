using System;

namespace DemoAuth.Api.Models;

public sealed class AppUser

{
    public int Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public bool IsTi { get; set; }
}

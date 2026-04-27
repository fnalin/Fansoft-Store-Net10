using System;

namespace DemoAuth.Api.Models;

public enum AccessLevel
{
    None = 0,
    Visualizador = 1,
    Editor = 2,
    Admin = 3
}
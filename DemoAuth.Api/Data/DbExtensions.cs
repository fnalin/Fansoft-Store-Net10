using System;
using DemoAuth.Api.Models;

namespace DemoAuth.Api.Data;

public static class DbExtensions
{
    public static void SeedDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (db.Users.Any())
        return;

    db.Users.AddRange(
        new AppUser
        {
            Id = 1,
            UserName = "fulano",
            DisplayName = "Fulano Financeiro Viewer",
            IsTi = false
        },

        new AppUser
        {
            Id = 2,
            UserName = "ciclano",
            DisplayName = "Ciclano Financeiro Editor",
            IsTi = false
        },

        new AppUser
        {
            Id = 3,
            UserName = "ze",
            DisplayName = "Zé Financeiro Admin",
            IsTi = false
        },

        new AppUser
        {
            Id = 4,
            UserName = "maria",
            DisplayName = "Maria Compras Editor",
            IsTi = false
        },

        new AppUser
        {
            Id = 5,
            UserName = "ti",
            DisplayName = "Usuário TI",
            IsTi = true
        });

    db.UserPermissions.AddRange(

        new UserPermission
        {
            Id = 1,
            UserName = "fulano",
            Module = "Financeiro",
            Level = AccessLevel.Visualizador,
            CanHandleTickets = false
        },

        new UserPermission
        {
            Id = 2,
            UserName = "ciclano",
            Module = "Financeiro",
            Level = AccessLevel.Editor,
            CanHandleTickets = true
        },

        new UserPermission
        {
            Id = 3,
            UserName = "ze",
            Module = "Financeiro",
            Level = AccessLevel.Admin,
            CanHandleTickets = false
        },

        new UserPermission
        {
            Id = 6,
            UserName = "ze",
            Module = "Compras",
            Level = AccessLevel.Admin,
            CanHandleTickets = true
        },

        new UserPermission
        {
            Id = 4,
            UserName = "maria",
            Module = "Compras",
            Level = AccessLevel.Editor,
            CanHandleTickets = true
        },

        new UserPermission
        {
            Id = 7,
            UserName = "maria",
            Module = "Financeiro",
            Level = AccessLevel.Visualizador,
            CanHandleTickets = false
        },

        new UserPermission
        {
            Id = 5,
            UserName = "fulano",
            Module = "Compras",
            Level = AccessLevel.Visualizador,
            CanHandleTickets = true
        }
    );

    db.Products.AddRange(

        new Product
        {
            Id = 1,
            Name = "Notebook",
            Price = 4500
        },

        new Product
        {
            Id = 2,
            Name = "Monitor",
            Price = 1200
        },

        new Product
        {
            Id = 3,
            Name = "Teclado",
            Price = 250
        });

    db.PurchaseOrders.AddRange(

        new PurchaseOrder
        {
            Id = 1,
            Description = "Compra de material de escritório",
            Amount = 800
        },

        new PurchaseOrder
        {
            Id = 2,
            Description = "Compra de equipamentos",
            Amount = 5000
        });

    db.SaveChanges();

}
}

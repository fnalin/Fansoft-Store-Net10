using System;
using DemoAuth.Api.Data;
using DemoAuth.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoAuth.Api.Endpoints;

public static class ComprasEndpoint
{

    public static void MapComprasEndpoints(this IEndpointRouteBuilder app)
    {
        var comprasGroup = app.MapGroup("/api/compras").WithTags("Compras");

        comprasGroup.MapGet("/pedidos", async (AppDbContext db) =>
        {
            var orders = await db.PurchaseOrders
                .AsNoTracking()
                .ToListAsync();

            return Results.Ok(orders);
        })
        .RequireAuthorization("Compras.Visualizar");

        comprasGroup.MapPut("/pedidos/{id:int}", async (
            int id,
            UpdatePurchaseOrderRequest request,
            AppDbContext db) =>
        {
            var order = await db.PurchaseOrders.FindAsync(id);
            if (order is null)
                return Results.NotFound();

            order.Description = request.Description;
            order.Amount = request.Amount;
            await db.SaveChangesAsync();
            return Results.Ok(order);
        })
        .RequireAuthorization("Compras.Editar");

        comprasGroup.MapDelete("/pedidos/{id:int}", async (
            int id,
            AppDbContext db) =>
        {
            var order = await db.PurchaseOrders.FindAsync(id);
            if (order is null)
                return Results.NotFound();

            db.PurchaseOrders.Remove(order);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .RequireAuthorization("Compras.Admin");


        comprasGroup.MapGet("/chamados", () =>
        {
            var chamados = new[]
            {
        new { Id = 1, Titulo = "Chamado compras 001" },
        new { Id = 2, Titulo = "Chamado compras 002" }
            };

            return Results.Ok(chamados);
        })
        .RequireAuthorization("Compras.Atendimento");

    }

}

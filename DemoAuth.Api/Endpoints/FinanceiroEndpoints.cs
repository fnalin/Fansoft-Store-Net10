using System;
using DemoAuth.Api.Data;
using DemoAuth.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoAuth.Api.Endpoints;

public static class FinanceiroEndpoints
{
    public static void MapFinanceiroEndpoints(this IEndpointRouteBuilder app)
    {
        var financeiroGroup = app.MapGroup("/api/financeiro").WithTags("Financeiro");
    
        financeiroGroup.MapGet("/produtos", async (AppDbContext db) =>
            {
                var products = await db.Products
                    .AsNoTracking()
                    .ToListAsync();
                return Results.Ok(products);
            })
            .RequireAuthorization("Financeiro.Visualizar");

        financeiroGroup.MapGet("/produtos/{id:int}", async (
            int id,
            AppDbContext db) =>
            {

                var product = await db.Products
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                return product is null
                    ? Results.NotFound()
                    : Results.Ok(product);
            })
            .RequireAuthorization("Financeiro.Visualizar");

        financeiroGroup.MapPut("/produtos/{id:int}", async (
            int id,
            UpdateProductRequest request,
            AppDbContext db) =>
        {

            var product = await db.Products.FindAsync(id);
            if (product is null)
                return Results.NotFound();

            product.Name = request.Name;
            product.Price = request.Price;

            await db.SaveChangesAsync();
            return Results.Ok(product);
        })
        .RequireAuthorization("Financeiro.Editar");

        financeiroGroup.MapDelete("/produtos/{id:int}", async (
            int id,
            AppDbContext db) =>
        {

            var product = await db.Products.FindAsync(id);

            if (product is null)
                return Results.NotFound();

            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return Results.NoContent();

        })
        .RequireAuthorization("Financeiro.Admin");

        financeiroGroup.MapGet("/chamados", () =>
        {
            var chamados = new[]
            {
                new { Id = 1, Titulo = "Chamado financeiro 001" },
                new { Id = 2, Titulo = "Chamado financeiro 002" }
            };

            return Results.Ok(chamados);
        })
        .RequireAuthorization("Financeiro.Atendimento");
    }

}

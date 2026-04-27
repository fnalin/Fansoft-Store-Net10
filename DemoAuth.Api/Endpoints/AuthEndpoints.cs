using DemoAuth.Api.Data;
using DemoAuth.Api.Models;
using DemoAuth.Api.Services;

namespace DemoAuth.Api.Endpoints;

public static class AuthEndpoints
{

    const string issuer = "DemoAuth.Mvc";
    const string audience = "DemoAuth.Api";
    const string signingKey = "DEMO_SUPER_SECRET_KEY_123456789_DEMO_SUPER_SECRET_KEY";

    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var authGroup = app.MapGroup("/api/auth").WithTags("Authentication");

        authGroup.MapPost("/demo-token", (
                DemoTokenRequest request,
                AppDbContext db) =>
        {
            var user = db.Users.FirstOrDefault(x => x.UserName == request.UserName);

            if (user is null)
                return Results.NotFound("Usuário não encontrado");

            var token = DemoJwtTokenGenerator.Generate(
                userName: user.UserName,
                displayName: user.DisplayName,
                issuer: issuer,
                audience: audience,
                signingKey: signingKey,
                db: db);

            return Results.Ok(new { accessToken = token });
        });
    }
}

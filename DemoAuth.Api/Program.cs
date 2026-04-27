using System.Text;
using DemoAuth.Api.Data;
using DemoAuth.Api.Endpoints;
using DemoAuth.Api.Models;
using DemoAuth.Api.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

const string issuer = "DemoAuth.Mvc";
const string audience = "DemoAuth.Api";
const string signingKey = "DEMO_SUPER_SECRET_KEY_123456789_DEMO_SUPER_SECRET_KEY";

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("DemoAuthDb");
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(signingKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Financeiro.Visualizar", policy =>
        policy.Requirements.Add(new ModuleAccessRequirement("Financeiro", AccessLevel.Visualizador)));
    options.AddPolicy("Financeiro.Editar", policy =>
        policy.Requirements.Add(new ModuleAccessRequirement("Financeiro", AccessLevel.Editor)));
    options.AddPolicy("Financeiro.Admin", policy =>
        policy.Requirements.Add(new ModuleAccessRequirement("Financeiro", AccessLevel.Admin)));
        options.AddPolicy("Financeiro.Atendimento", policy =>
    policy.Requirements.Add(
        new ModuleActionRequirement("Financeiro", "Atendimento")));
    

    options.AddPolicy("Compras.Visualizar", policy =>
        policy.Requirements.Add(new ModuleAccessRequirement("Compras", AccessLevel.Visualizador)));
    options.AddPolicy("Compras.Editar", policy =>
        policy.Requirements.Add(new ModuleAccessRequirement("Compras", AccessLevel.Editor)));
    options.AddPolicy("Compras.Admin", policy =>
        policy.Requirements.Add(new ModuleAccessRequirement("Compras", AccessLevel.Admin)));
    options.AddPolicy("Compras.Atendimento", policy =>
        policy.Requirements.Add(
            new ModuleActionRequirement("Compras", "Atendimento")));
});

builder.Services.AddScoped<IAuthorizationHandler, ModuleAccessHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ModuleActionHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DemoAuth API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Informe o token JWT no formato: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });
});

var app = builder.Build();

DbExtensions.SeedDatabase(app);

app.UseSwagger();

app.UseSwaggerUI();

app.UseAuthentication();

app.UseAuthorization();

app.MapGet("/", () => "DemoAuth API running");
app.MapAuthEndpoints();
app.MapFinanceiroEndpoints();
app.MapComprasEndpoints();


app.Run();

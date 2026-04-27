using DemoAuth.Mvc.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ApiTokenService>();
builder.Services.AddScoped<ApiAuthorizationHandler>();

builder.Services.AddHttpClient("DemoAuthApi", client =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"]!;
    client.BaseAddress = new Uri(baseUrl);
}).AddHttpMessageHandler<ApiAuthorizationHandler>();

builder.Services.AddScoped<FinanceiroApiService>();
builder.Services.AddScoped<ComprasApiService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

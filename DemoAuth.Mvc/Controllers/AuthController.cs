using System.Security.Claims;
using DemoAuth.Mvc.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace DemoAuth.Mvc.Controllers;

public sealed class AuthController : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var user = MockAdUsers.GetUser(model.UserName, model.Password);

        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserName),
            new(ClaimTypes.Name, user.DisplayName)
        };

        

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}

public static class MockAdUsers
{
    public static MockAdUser? GetUser(string userName, string password)
    {
        if (password != "123")
            return null;

        return userName.ToLowerInvariant() switch
        {
            "fulano" => new MockAdUser("fulano", "Fulano"),
            "ciclano" => new MockAdUser("ciclano", "Ciclano"),
            "ze" => new MockAdUser("ze", "Zé"),
            "maria" => new MockAdUser("maria", "Maria"),
            "ti" => new MockAdUser("ti", "TI God Mode"),
            _ => null
        };
    }
}

public sealed record MockAdUser(
    string UserName,
    string DisplayName);
using DemoAuth.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoAuth.Mvc.Controllers;

[Authorize]
public sealed class ComprasController : Controller
{
    private readonly ComprasApiService _comprasApiService;

    public ComprasController(ComprasApiService comprasApiService)
    {
        _comprasApiService = comprasApiService;
    }

    public async Task<IActionResult> Pedidos()
    {
        var result = await _comprasApiService.GetPedidosAsync();

        if (result.IsForbidden)
            return View("Forbidden");

        if (result.IsUnauthorized)
            return RedirectToAction("Login", "Auth");

        return View(result.Data);
    }

    public async Task<IActionResult> Chamados()
    {
        var result = await _comprasApiService.GetChamadosAsync();

        if (result.IsForbidden)
            return View("Forbidden");

        if (result.IsUnauthorized)
            return RedirectToAction("Login", "Auth");

        return View(result.Data);
    }
}
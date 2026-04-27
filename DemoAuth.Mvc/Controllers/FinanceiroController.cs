using DemoAuth.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoAuth.Mvc.Controllers;

[Authorize]
public sealed class FinanceiroController : Controller
{
    private readonly FinanceiroApiService _financeiroApiService;

    public FinanceiroController(FinanceiroApiService financeiroApiService)
    {
        _financeiroApiService = financeiroApiService;
    }

    public async Task<IActionResult> Produtos()
    {
        var result = await _financeiroApiService.GetProdutosAsync();

        if (result.IsForbidden)
            return View("Forbidden");

        if (result.IsUnauthorized)
            return RedirectToAction("Login", "Auth");

        return View(result.Data);
    }

    public async Task<IActionResult> Chamados()
    {
        var result = await _financeiroApiService.GetChamadosAsync();

        if (result.IsForbidden)
            return View("Forbidden");

        if (result.IsUnauthorized)
            return RedirectToAction("Login", "Auth");

        return View(result.Data);
    }
}
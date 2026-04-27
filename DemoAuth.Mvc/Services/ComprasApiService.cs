using System.Net;
using DemoAuth.Mvc.Models;

namespace DemoAuth.Mvc.Services;

public sealed class ComprasApiService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ComprasApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ApiResult<List<PurchaseOrderViewModel>>> GetPedidosAsync()
    {
        var client = _httpClientFactory.CreateClient("DemoAuthApi");

        var response = await client.GetAsync("/api/compras/pedidos");

        if (response.StatusCode == HttpStatusCode.Forbidden)
            return ApiResult<List<PurchaseOrderViewModel>>.Forbidden();

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return ApiResult<List<PurchaseOrderViewModel>>.Unauthorized();

        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<List<PurchaseOrderViewModel>>();

        return ApiResult<List<PurchaseOrderViewModel>>.Success(data ?? []);
    }

    public async Task<ApiResult<List<TicketViewModel>>> GetChamadosAsync()
    {
        var client = _httpClientFactory.CreateClient("DemoAuthApi");

        var response = await client.GetAsync("/api/compras/chamados");

        if (response.StatusCode == HttpStatusCode.Forbidden)
            return ApiResult<List<TicketViewModel>>.Forbidden();

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return ApiResult<List<TicketViewModel>>.Unauthorized();

        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<List<TicketViewModel>>();

        return ApiResult<List<TicketViewModel>>.Success(data ?? []);
    }
}


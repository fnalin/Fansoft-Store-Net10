using System.Net;
using DemoAuth.Mvc.Models;

namespace DemoAuth.Mvc.Services;

public sealed class FinanceiroApiService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public FinanceiroApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ApiResult<List<ProductViewModel>>> GetProdutosAsync()
    {
        var client = _httpClientFactory.CreateClient("DemoAuthApi");

        var response = await client.GetAsync("/api/financeiro/produtos");

        if (response.StatusCode == HttpStatusCode.Forbidden)
            return ApiResult<List<ProductViewModel>>.Forbidden();

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return ApiResult<List<ProductViewModel>>.Unauthorized();

        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<List<ProductViewModel>>();

        return ApiResult<List<ProductViewModel>>.Success(data ?? []);
    }

    public async Task<ApiResult<List<TicketViewModel>>> GetChamadosAsync()
{
    var client = _httpClientFactory.CreateClient("DemoAuthApi");

    var response = await client.GetAsync("/api/financeiro/chamados");

    if (response.StatusCode == HttpStatusCode.Forbidden)
        return ApiResult<List<TicketViewModel>>.Forbidden();

    if (response.StatusCode == HttpStatusCode.Unauthorized)
        return ApiResult<List<TicketViewModel>>.Unauthorized();

    response.EnsureSuccessStatusCode();

    var data = await response.Content.ReadFromJsonAsync<List<TicketViewModel>>();

    return ApiResult<List<TicketViewModel>>.Success(data ?? []);
}
}




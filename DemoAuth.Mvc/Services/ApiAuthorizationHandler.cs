using System.Net.Http.Headers;

namespace DemoAuth.Mvc.Services;

public sealed class ApiAuthorizationHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApiTokenService _apiTokenService;

    public ApiAuthorizationHandler(
        IHttpContextAccessor httpContextAccessor,
        ApiTokenService apiTokenService)
    {
        _httpContextAccessor = httpContextAccessor;
        _apiTokenService = apiTokenService;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user?.Identity?.IsAuthenticated == true)
        {
            var token = _apiTokenService.GenerateToken(user);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
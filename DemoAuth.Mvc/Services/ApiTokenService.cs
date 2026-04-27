using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DemoAuth.Mvc.Services;

public sealed class ApiTokenService
{
    private readonly IConfiguration _configuration;

    public ApiTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(ClaimsPrincipal user)
    {
        var userName = user.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var displayName = user.FindFirstValue(ClaimTypes.Name) ?? userName;

        var issuer = _configuration["ApiSettings:Issuer"]!;
        var audience = _configuration["ApiSettings:Audience"]!;
        var clientId = _configuration["ApiSettings:ClientId"]!;
        var signingKey = _configuration["ApiSettings:SigningKey"]!;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userName),
            new(ClaimTypes.Name, displayName),
            new("sub", userName),
            new("client_id", clientId),
            new("typ", "mvc_delegation")
        };


        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}
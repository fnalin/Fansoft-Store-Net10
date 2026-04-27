using System;
using System.Security.Claims;
using System.Text;
using DemoAuth.Api.Data;
using Microsoft.IdentityModel.Tokens;

namespace DemoAuth.Api.Services;

public static class DemoJwtTokenGenerator
{
    public static string Generate(
        string userName,
        string displayName,
        string issuer,
        string audience,
        string signingKey,
        AppDbContext db)
    {

        var permissions = db.UserPermissions.Where(x => x.UserName == userName).ToList();

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userName),
                new Claim(ClaimTypes.Name, displayName),
                new Claim("sub", userName),
                new Claim("client_id", "demo-mvc"),
                new Claim("typ", "mvc_delegation")
            };

        // GOD MODE
        var user = db.Users.FirstOrDefault(x => x.UserName == userName);

        if (user?.IsTi == true)
            claims.Add(new Claim("god_mode", "true"));


        // PERMISSÕES
        foreach (var perm in permissions)
        {
            claims.Add(new Claim("permission", $"{perm.Module}:{(int)perm.Level}"));

            if (perm.CanHandleTickets)
                claims.Add(new Claim("permission", $"{perm.Module}:Atendimento"));
        }

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(signingKey));

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256);


        var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials);

        return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler()
            .WriteToken(token);
    }

}

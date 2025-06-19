using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Volo.Abp.DependencyInjection;

namespace Acme.BookStore.Auth;

public class JwtGenerator : ITransientDependency
{
    private readonly IConfiguration _configuration;

    public JwtGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(string username, string tenant)
    {
        var secretKey = _configuration["TempAuth:SecretKey"] ?? "ThisIsMySecretKeyForJwtTokenGeneration12345";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim("tenant", tenant),
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["TempAuth:Issuer"] ?? "BookStore",
            audience: _configuration["TempAuth:Audience"] ?? "BookStore",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool ValidateCredentials(string username, string password, string tenant)
    {
        // 臨時驗證邏輯，實際環境中應該連接到真正的用戶資料庫
        var validUsers = new Dictionary<string, string>
        {
            { "admin", "password123" },
            { "user1", "password456" },
            { "user2", "password789" }
        };

        return validUsers.ContainsKey(username) && validUsers[username] == password;
    }
}
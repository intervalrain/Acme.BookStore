using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

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
            // 標準Claims
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.NameIdentifier, username),
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            
            // ABP框架需要的Claims
            new Claim(AbpClaimTypes.UserId, Guid.NewGuid().ToString()), // 模擬用戶ID
            new Claim(AbpClaimTypes.UserName, username),
            new Claim(AbpClaimTypes.Name, username),
            new Claim(AbpClaimTypes.TenantId, GetTenantId(tenant)),
            new Claim("tenant", tenant), // 保留原有的tenant claim
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

    private string GetTenantId(string tenantName)
    {
        // 模擬租戶ID映射，實際環境中應該從數據庫查詢
        var tenantMappings = new Dictionary<string, string>
        {
            { "default", "3fa85f64-5717-4562-b3fc-2c963f66afa6" },
            { "tenant1", "3fa85f64-5717-4562-b3fc-2c963f66afa7" },
            { "tenant2", "3fa85f64-5717-4562-b3fc-2c963f66afa8" }
        };

        return tenantMappings.GetValueOrDefault(tenantName ?? "default", "3fa85f64-5717-4562-b3fc-2c963f66afa6");
    }
}
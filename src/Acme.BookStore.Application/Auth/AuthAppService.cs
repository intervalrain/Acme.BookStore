using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Auth;

[AllowAnonymous]
public class AuthAppService : ApplicationService, IAuthAppService
{
    private readonly JwtGenerator _jwtGenerator;

    public AuthAppService(JwtGenerator jwtGenerator)
    {
        _jwtGenerator = jwtGenerator;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto input)
    {
        if (!_jwtGenerator.ValidateCredentials(input.Username, input.Password, input.Tenant ?? "default"))
        {
            throw new UserFriendlyException("Invalid username or password");
        }

        var token = _jwtGenerator.GenerateToken(input.Username, input.Tenant ?? "default");

        return await Task.FromResult(new LoginResponseDto
        {
            Token = token,
            Username = input.Username,
            Tenant = input.Tenant,
            ExpiresAt = DateTime.UtcNow.AddHours(24)
        });
    }
}
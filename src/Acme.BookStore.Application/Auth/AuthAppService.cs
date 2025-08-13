using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Auth;

public class AuthAppService : ApplicationService, IAuthAppService
{
    private readonly JwtGenerator _jwtGenerator;

    public AuthAppService(JwtGenerator jwtGenerator)
    {
        _jwtGenerator = jwtGenerator;
    }


    [AllowAnonymous]
    public async Task<string> LoginAsync(LoginDto input)
    {
        if (!_jwtGenerator.ValidateCredentials(input.Username, input.Password, input.Tenant ?? "default"))
        {
            throw new UserFriendlyException("Invalid username or password");
        }

        var token = _jwtGenerator.GenerateToken(input.Username, input.Tenant ?? "default");

        return await Task.FromResult(token);
    }

    [Authorize(AuthenticationSchemes = "TempJwt")]
    public UserInfo GetCurrentUser()
    {
        return new UserInfo
        {
            Username = CurrentUser.Name,
            Tenant = CurrentTenant.Name,
            Claims = CurrentUser.GetAllClaims().Select(claim => claim.ToString()).ToArray()
        };
    }

    [Authorize(AuthenticationSchemes = "TempJwt")]
    public GetProtectedData GetProtectedData()
    {
        return new GetProtectedData
        {
            Message = "This is protected data",
            User = CurrentUser.Name,
            Tenant = CurrentTenant.Name,
            Timestamp = DateTime.UtcNow
        };
    }
}

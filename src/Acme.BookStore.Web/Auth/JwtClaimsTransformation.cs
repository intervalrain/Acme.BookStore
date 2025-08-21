using Microsoft.AspNetCore.Authentication;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Acme.BookStore.Web.Auth;

public class JwtClaimsTransformation : IClaimsTransformation, ITransientDependency
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        // 檢查是否是JWT Bearer認證並且已經認證
        if (principal.Identity?.IsAuthenticated == true && 
            (principal.Identity.AuthenticationType == "AuthenticationTypes.Federation" || 
             principal.Identity.AuthenticationType == "Bearer" ||
             principal.Identities.Any(i => i.AuthenticationType == "TempJwt")))
        {
            var identity = principal.Identity as ClaimsIdentity ?? 
                          principal.Identities.FirstOrDefault(i => i.AuthenticationType == "TempJwt");
            
            if (identity != null)
            {
                // 確保有必要的ABP Claims
                EnsureAbpClaims(identity);
            }
        }

        return Task.FromResult(principal);
    }

    private void EnsureAbpClaims(ClaimsIdentity identity)
    {
        // 確保UserId claim存在
        if (identity.FindFirst(AbpClaimTypes.UserId) == null)
        {
            var nameIdentifier = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(nameIdentifier))
            {
                identity.AddClaim(new Claim(AbpClaimTypes.UserId, nameIdentifier));
            }
        }

        // 確保UserName claim存在
        if (identity.FindFirst(AbpClaimTypes.UserName) == null)
        {
            var name = identity.FindFirst(ClaimTypes.Name)?.Value;
            if (!string.IsNullOrEmpty(name))
            {
                identity.AddClaim(new Claim(AbpClaimTypes.UserName, name));
            }
        }

        // 確保Name claim存在
        if (identity.FindFirst(AbpClaimTypes.Name) == null)
        {
            var name = identity.FindFirst(ClaimTypes.Name)?.Value;
            if (!string.IsNullOrEmpty(name))
            {
                identity.AddClaim(new Claim(AbpClaimTypes.Name, name));
            }
        }
    }
}
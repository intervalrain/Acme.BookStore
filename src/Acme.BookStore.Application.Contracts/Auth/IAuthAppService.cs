using System.Threading.Tasks;

using Volo.Abp.Application.Services;

namespace Acme.BookStore.Auth;

public interface IAuthAppService : IApplicationService
{
    Task<string> LoginAsync(LoginDto input);
    UserInfo GetCurrentUser();
    GetProtectedData GetProtectedData();
}
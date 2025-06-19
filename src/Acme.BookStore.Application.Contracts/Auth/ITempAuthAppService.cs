using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Auth;

public interface IAuthAppService : IApplicationService
{
    Task<LoginResponseDto> LoginAsync(LoginDto input);
}
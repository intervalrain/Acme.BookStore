using System.Threading.Tasks;

using Acme.BookStore.Status.Dtos;

namespace Acme.BookStore.Status;

public interface IStatusAppService
{
    Task<string> GetStatusAsync(ExceptionDto input);

    Task<object> CreateAsync(CreateResourceDto input);
    Task<object> GetAsync(int id);
    Task<object> UpdateAsync(int id, UpdateResourceDto input);
    Task<object> DeleteAsync(int id);
}
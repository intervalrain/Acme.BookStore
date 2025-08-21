using System.Threading.Tasks;

using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Acme.BookStore.EntityFrameworkCore;

public class TenantConnectionStringResolver : ITenantResolver, ITransientDependency
{
    public Task<TenantResolveResult> ResolveTenantIdOrNameAsync()
    {
        throw new System.NotImplementedException();
    }
}
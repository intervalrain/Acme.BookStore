using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using EdgeSync.Common.Net.Filters;

namespace EdgeSync.Common.Net.Extensions;

/// <summary>
/// Extension methods for IServiceCollection to configure EdgeSync exception handling for ABP applications
/// </summary>
public static class AbpServiceCollectionExtensions
{
    /// <summary>
    /// Adds EdgeSync exception handling middleware for ABP Framework applications
    /// </summary>
    public static IServiceCollection AddAbpEdgeSyncExceptionHandling(this IServiceCollection services)
    {
        services.Configure<MvcOptions>(options =>
        {
            // Add exception filter for converting exceptions to RFC 9457 Problem Details
            options.Filters.Add<EdgeSyncExceptionFilter>();
        });

        return services;
    }
}
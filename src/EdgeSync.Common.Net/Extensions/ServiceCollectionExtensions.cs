using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using EdgeSync.Common.Net.Filters;
using EdgeSync.Common.Net.Options;

namespace EdgeSync.Common.Net.Extensions;

/// <summary>
/// Extension methods for IServiceCollection to configure EdgeSync exception handling
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds EdgeSync exception handling middleware for ASP.NET Core applications
    /// </summary>
    public static IServiceCollection AddEdgeSyncExceptionHandling(this IServiceCollection services)
    {
        services.Configure<MvcOptions>(options =>
        {
            // Add exception filter for converting exceptions to RFC 9457 Problem Details
            options.Filters.Add<EdgeSyncExceptionFilter>();
        });

        return services;
    }
    
    /// <summary>
    /// Configures EdgeSync exception handling options
    /// </summary>
    public static IServiceCollection ConfigureEdgeSyncExceptionHandling(this IServiceCollection services, Action<EdgeSyncExceptionHandlingOptions>? configureOptions = null)
    {
        if (configureOptions != null)
        {
            services.Configure(configureOptions);
        }
        
        return services.AddEdgeSyncExceptionHandling();
    }
}
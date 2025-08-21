using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.CallerInfo;
using Serilog.Events;

namespace Acme.BookStore.Web;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        var appName = "Acme.BookStore";
        Log.Logger = new LoggerConfiguration()
            // .WriteTo.Async(c => c.File("Logs/logs.txt"))
            .WriteTo.Async(c => c.Console())
            .CreateBootstrapLogger();

        try
        {
            // var outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] [{Namespace}::{SourceFile}::{Method} @ {LineNumber}] {Message:lj}{NewLine}{Exception}";
            var outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";
            Log.Information("Starting web host.");
            var builder = WebApplication.CreateBuilder(args);
            builder.Host
                .AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseSerilog((context, services, loggerConfiguration) =>
                {
                    loggerConfiguration
                    #if DEBUG
                        .MinimumLevel.Debug()
                    #else
                        .MinimumLevel.Information()
                    #endif
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        // .Enrich.WithDemystifiedStackTraces()
                        // .Enrich.WithAssemblyName()
                        .Enrich.WithCallerInfo(
                            includeFileInfo: true, 
                            assemblyPrefix: appName,
                            filePathDepth: 3)
                        // .WriteTo.Async(c => c.File("Logs/logs.txt"))
                        .WriteTo.Async(c => c.Console(outputTemplate: outputTemplate))
                        .WriteTo.Async(c => c.AbpStudio(services));
                });
            await builder.AddApplicationAsync<BookStoreWebModule>(options => options.ApplicationName = appName);
            var app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}

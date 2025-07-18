using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace App.Console;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        try
        {
            await CreateHostBuilder(args).RunConsoleAsync();
            return 0;
        }
        catch (Exception ex)
        {
            return 1;
        }
        finally
        {
        }
    }

    internal static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseEnvironment("Development")
            .ConfigureServices((hostContext, services) =>
            {
                services.AddApplication<ConsoleAppModule>();
            });
}

public class ConsoleAppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHostedService<MyProjectNameHostedService>();
        context.Services.AddTransient<ISuma, Suma>();
    }
}
public interface ISuma
{
    int Sum();
}

public class Suma : ISuma
{
    public int Sum()
    {
        return 444;
    }
}
public class MyProjectNameHostedService : IHostedService
{
    private readonly IAbpApplicationWithExternalServiceProvider _application;
    private readonly ILogger<MyProjectNameHostedService> _logger;
    private readonly ISuma _suma;
    private readonly IServiceProvider _serviceProvider;

    public MyProjectNameHostedService(
        IAbpApplicationWithExternalServiceProvider application,
        ILogger<MyProjectNameHostedService> logger,
        ISuma suma,
        IServiceProvider serviceProvider)
    {
        _application = application;
        _logger = logger;
        _suma = suma;
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _application.Initialize(_serviceProvider);
        _logger.LogInformation("MyProjectName module is initialized.");
        _logger.LogInformation(_suma.Sum().ToString());
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _application.Shutdown();
        return Task.CompletedTask;
    }
}
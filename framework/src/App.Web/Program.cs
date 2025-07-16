using Microsoft.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;

namespace App.Web;

[Route("/")]
[ApiController]
public class AppController : Controller
{
    private readonly IHostEnvironment _environment;
    private readonly ISuma _suma;
    private readonly ILogger<AppController> _logger;

    public AppController(IHostEnvironment environment, ISuma suma, ILogger<AppController> logger)
    {
        _environment = environment;
        _suma = suma;
        _logger = logger;
    }

    [HttpGet()]
    public App Get()
    {
        _logger.LogInformation("Call to sum : {0}", _suma.Sum());
        return new App() { Version = "1.0.0", Environment = _environment.EnvironmentName };
    }
}

public class App
{
    public string Version { get; set; }
    public string Environment { get; set; }
}

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var startup = new Startup();
        startup.ConfigureServices(builder.Services);

        var app = builder.Build();
        startup.Configure(app, app.Environment);

        app.Run();
    }
}

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddApplication<AppModule>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
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
public class AppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<ISuma, Suma>();
    }
}
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;

namespace App.Web;

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

public class AppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddControllers();
    }
}

public interface ISuma
{
    int Sum();
}

public class Suma : ISuma, ITransientDependency
{
    public int Sum()
    {
        return 444;
    }
}

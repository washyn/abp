using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;

namespace App.Web.Test;

#region Other test

public class ServerTest
{
    public HttpClient HttpClient { get; set; }

    public ServerTest()
    {
        var webAppFactory = new WebApplicationFactory<Program>();
        HttpClient = webAppFactory.CreateDefaultClient();
    }

    [Fact]
    public async Task GetAppTest()
    {
        var response = await HttpClient.GetAsync("/");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        result.ShouldNotBeNullOrWhiteSpace();
        result.ShouldContain("1.0.0");
    }
}

#endregion

public class DiTest : AbpAspNetCoreTestBase
{
    private readonly ISuma _suma;

    public DiTest()
    {
        _suma = this.Provider.GetRequiredService<ISuma>();
    }

    [Fact]
    public void TestSuma()
    {
        var sum = _suma.Sum();
        sum.ShouldBe(444);
    }
}

public class ProgramTest : AbpAspNetCoreTestBase
{
    [Fact]
    public async Task Get_Root_Returns_Hello_World()
    {
        var response = await Client.GetStringAsync("/");
        response.ShouldContain("1.0.0");
    }
}

public class StartupTest
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddApplicationPart(typeof(AppController).Assembly);
        services.AddTransient<ISuma, Suma>();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

public abstract class AbpAspNetCoreTestBase : AbpAspNetCoreTestBase<StartupTest>
{
}

public abstract class AbpAspNetCoreTestBase
    <TStartup>
    : IDisposable
    where TStartup : class
{
    protected TestServer Server { get; }
    protected HttpClient Client { get; }
    protected IServiceProvider Provider { get; }
    private readonly IHost _host;

    protected AbpAspNetCoreTestBase()
    {
        var builder = CreateHostBuilder();
        _host = builder.Build();
        _host.Start();
        Server = _host.GetTestServer();
        Client = _host.GetTestClient();
        Provider = Server.Services;
    }

    protected virtual IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<TStartup>();
                webBuilder.UseTestServer();
            });
    }

    public void Dispose()
    {
        _host?.Dispose();
    }
}

// TODO: probar si se puede agregar otro startup propio aqui... y ver si funca...
// y tercero con el startup adicional en el proyecto de test, en este startup de test deberia incluirse este
// modulo WebAppTestModule
// al igual que n el proyecto web usar el module solo para la injecccion de dependencia y probar...
// [Dependency(ReplaceServices = true, TryRegister = true)]
// REvisar porque el controller no se esta tomando y por ese motivo esta lanzando not found,
// hacerlo sin abp
// usarlo solo el startup sin abp module...
// [DependsOn(typeof(AppModule))]
// public class WebAppTestModule : AbpModule
// {
//     public override void ConfigureServices(ServiceConfigurationContext context)
//     {
//     }
// }

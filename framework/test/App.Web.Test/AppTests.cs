using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Shouldly;

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
        var response = await HttpClient.GetAsync("/api/app");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        result.ShouldNotBeNullOrWhiteSpace();
        result.ShouldContain("1.0.0");
    }
}

#endregion

public class ProgramTest : AbpAspNetCoreTestBase
{
    [Fact]
    public async Task Get_Root_Returns_Hello_World()
    {
        var response = await Client.GetStringAsync("/api/app");
        response.ShouldContain("1.0.0");
    }
}

public abstract class AbpAspNetCoreTestBase : AbpAspNetCoreTestBase<Startup>
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
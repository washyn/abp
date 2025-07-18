using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;

namespace App.Console.Tests;

[DependsOn(typeof(AbpTestBaseModule))]
[DependsOn(typeof(ConsoleAppModule))]
public class ConsoleTestsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Replace(ServiceDescriptor.Transient<ISuma, TestClass>());
    }
}

public class SumService_Tests : AbpIntegratedTest<ConsoleTestsModule>
{
    private readonly ISuma _suma;

    public SumService_Tests()
    {
        _suma = GetRequiredService<ISuma>();
    }

    [Fact]
    public void Should_Sum_Test()
    {
        var sum = _suma.Sum();
        sum.ShouldBe(69);
    }
}

public class TestClass : ISuma
{
    public int Sum()
    {
        return 69;
    }
}
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace LSTC.Shared.Tests;

public class ServiceCollectionExtensionsTests
{
    public interface ITestInterface { }
    public class DirectImplementation : ITestInterface
    {
        public class SubclassImplementation : ITestInterface
        {
        }
    }

    [Fact]
    public void AddScoped()
    {
        var serviceProvider = new ServiceCollection()
            .AddScopedFromAssembly<ITestInterface>(typeof(DirectImplementation).Assembly)
            .BuildServiceProvider();

        var instances = serviceProvider.GetServices<ITestInterface>();

        Assert.NotNull(instances);
        Assert.True(instances.Count() == 2);
        Assert.NotSame(instances.First(), instances.Last());
    }
}
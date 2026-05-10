using Xunit;
using Microsoft.Extensions.DependencyInjection;
using LSTC.Shared.CQS.Commands;

namespace LSTC.Shared.Tests;

public class CommandResolverTests
{
    public class TestCommand : ICommand
    {
    }

    public class TestCommandHandler : ICommandHandler<TestCommand>
    {
        public Task ExecuteAsync(TestCommand command)
        {
            return Task.CompletedTask;
        }
    }

    public class DuplicateCommandHandler : ICommandHandler<TestCommand>
    {
        public Task ExecuteAsync(TestCommand command)
        {
            return Task.CompletedTask;
        }
    }

    [Fact]
    public void Command_handler_not_registered()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<ICommandResolver, ServiceProviderCommandResolver>()
            .BuildServiceProvider();
        var resolver = serviceProvider.GetService<ICommandResolver>()!;
        
        Assert.Throws<InvalidOperationException>(() => resolver.Resolve<TestCommand>());
    }

    [Fact]
    public void Command_handler_registered()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<ICommandHandler<TestCommand>, TestCommandHandler>()
            .AddScoped<ICommandResolver, ServiceProviderCommandResolver>()
            .BuildServiceProvider();
        var resolver = serviceProvider.GetService<ICommandResolver>()!;
        
        var handler = resolver.Resolve<TestCommand>();
        Assert.NotNull(handler);
    }

    [Fact]
    public void Command_handler_registered_twice()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<ICommandHandler<TestCommand>, TestCommandHandler>()
            .AddScoped<ICommandHandler<TestCommand>, TestCommandHandler>()
            .AddScoped<ICommandResolver, ServiceProviderCommandResolver>()
            .BuildServiceProvider();
        var resolver = serviceProvider.GetService<ICommandResolver>()!;
        
        Assert.Throws<InvalidOperationException>(() => resolver.Resolve<TestCommand>());
    }

    [Fact]
    public void Command_handler_duplicate_registered()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<ICommandHandler<TestCommand>, TestCommandHandler>()
            .AddScoped<ICommandHandler<TestCommand>, DuplicateCommandHandler>()
            .AddScoped<ICommandResolver, ServiceProviderCommandResolver>()
            .BuildServiceProvider();
        var resolver = serviceProvider.GetService<ICommandResolver>()!;
        
        Assert.Throws<InvalidOperationException>(() => resolver.Resolve<TestCommand>());
    }
}

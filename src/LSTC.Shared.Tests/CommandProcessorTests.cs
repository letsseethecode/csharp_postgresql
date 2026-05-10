using Xunit;
using Microsoft.Extensions.DependencyInjection;
using LSTC.Shared.CQS.Commands;

namespace LSTC.Shared.Tests;

public class CommandProcessorTests
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

    [Fact]
    public void CommandProcessor_resolves()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<ICommandHandler<TestCommand>, TestCommandHandler>()
            .AddScoped<ICommandResolver, ServiceProviderCommandResolver>()
            .AddScoped<CommandProcessor, CommandProcessor>()
            .BuildServiceProvider();
        var processor = serviceProvider.GetService<CommandProcessor>()!;
        
        processor.ExecuteAsync(new TestCommand()).Wait();
    }
}

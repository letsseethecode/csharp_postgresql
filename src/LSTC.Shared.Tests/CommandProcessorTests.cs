using Xunit;
using Microsoft.Extensions.DependencyInjection;
using LSTC.Shared.CQS.Commands;
using System.ComponentModel.DataAnnotations;

namespace LSTC.Shared.Tests;

public class CommandProcessorTests
{
    public class TestCommand : ICommand
    {
        [Required, MinLength(3), MaxLength(100), RegularExpression(@"^[A-Z]+$")]
        public string? Name { get; set; }

        public TestCommand(string? name)
        {
            Name = name;
        }
    }

    public class TestCommandHandler : ICommandHandler<TestCommand>
    {
        public Task ExecuteAsync(TestCommand command)
        {
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task CommandProcessor_succeeds_validation()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<ICommandResolver, ServiceProviderCommandResolver>()
            .AddScoped<ICommandHandler<TestCommand>, TestCommandHandler>()
            .AddScoped<CommandProcessor, CommandProcessor>()
            .BuildServiceProvider();
        var processor = serviceProvider.GetService<CommandProcessor>()!;
        
        await processor.ExecuteAsync(new TestCommand("VALID"));
    }

    [Fact]
    public async Task CommandProcessor_fails_validation_with_multiple_errors()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<ICommandResolver, ServiceProviderCommandResolver>()
            .AddScoped<CommandProcessor, CommandProcessor>()
            .AddScoped<ICommandHandler<TestCommand>, TestCommandHandler>()
            .BuildServiceProvider();
        var processor = serviceProvider.GetService<CommandProcessor>()!;
        
        var ex = await Assert.ThrowsAsync<AggregateException>(() => processor.ExecuteAsync(new TestCommand("x")));
        Assert.All(ex.InnerExceptions, innerEx => Assert.IsType<ValidationException>(innerEx));
    }

        [Fact]
    public async Task CommandProcessor_fails_validation_with_single_error()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<ICommandResolver, ServiceProviderCommandResolver>()
            .AddScoped<CommandProcessor, CommandProcessor>()
            .AddScoped<ICommandHandler<TestCommand>, TestCommandHandler>()
            .BuildServiceProvider();
        var processor = serviceProvider.GetService<CommandProcessor>()!;
        
        await Assert.ThrowsAsync<ValidationException>(() => processor.ExecuteAsync(new TestCommand("X")));
    }
}

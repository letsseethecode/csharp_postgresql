using Xunit;
using Microsoft.Extensions.DependencyInjection;
using LSTC.Shared.CQS.Events;

namespace LSTC.Shared.Tests;

public class EventResolverTests
{
    public class TestEvent : IEvent
    {
    }

    public class TestEventHandler : IEventHandler<TestEvent>
    {
        public Task HandleAsync(TestEvent @event)
        {
            return Task.CompletedTask;
        }
    }

    public class DuplicateTestEventHandler : IEventHandler<TestEvent>
    {
        public Task HandleAsync(TestEvent @event)
        {
            return Task.CompletedTask;
        }
    }

    [Fact]
    public void Event_handler_not_registered()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IEventResolver, ServiceProviderEventResolver>()
            .BuildServiceProvider();
        var resolver = serviceProvider.GetService<IEventResolver>()!;
        
        var handlers = resolver.Resolve<TestEvent>();
        Assert.NotNull(handlers);
        Assert.Empty(handlers);
    }

    [Fact]
    public void Event_handler_registered()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IEventHandler<TestEvent>, TestEventHandler>()
            .AddScoped<IEventResolver, ServiceProviderEventResolver>()
            .BuildServiceProvider();
        var resolver = serviceProvider.GetService<IEventResolver>()!;
        
        var handlers = resolver.Resolve<TestEvent>();
        Assert.NotNull(handlers);
        Assert.Single(handlers);
    }

    [Fact]
    public void Event_handler_registered_twice()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IEventHandler<TestEvent>, TestEventHandler>()
            .AddScoped<IEventHandler<TestEvent>, DuplicateTestEventHandler>()
            .AddScoped<IEventResolver, ServiceProviderEventResolver>()
            .BuildServiceProvider();
        var resolver = serviceProvider.GetService<IEventResolver>()!;
        
        var handlers = resolver.Resolve<TestEvent>();
        Assert.NotNull(handlers);
        Assert.Equal(2, handlers.Count());
        Assert.True(handlers.Single(x => typeof(TestEventHandler).IsAssignableFrom(x.GetType())) is not null);
        Assert.True(handlers.Single(x => typeof(DuplicateTestEventHandler).IsAssignableFrom(x.GetType())) is not null);
    }
}
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using LSTC.Shared.CQS.Queries;
using System.ComponentModel.DataAnnotations;

namespace LSTC.Shared.Tests;

public class QueryResolverTests
{
    public class TestQuery : IQueryResults
    {
    }

    public class TestQueryHandler : IQueryHandler<TestQuery>
    {
        public Task<TestQuery> ExecuteAsync()
        {
            return Task.FromResult(new TestQuery());
        }
    }

    public class DuplicateQueryHandler : IQueryHandler<TestQuery>
    {
        public Task<TestQuery> ExecuteAsync()
        {
            return Task.FromResult(new TestQuery());
        }
    }

    public class TestQueryArgs : IQueryArgs
    {
    }

    public class TestQueryArgsHandler : IQueryHandler<TestQuery, TestQueryArgs>
    {
        public Task<TestQuery> ExecuteAsync(TestQueryArgs args)
        {
            return Task.FromResult(new TestQuery());
        }
    }

    public class DuplicateQueryArgsHandler : IQueryHandler<TestQuery, TestQueryArgs>
    {
        public Task<TestQuery> ExecuteAsync(TestQueryArgs args)
        {
            return Task.FromResult(new TestQuery());
        }
    }

    [Fact]
    public void Query_handler_not_registered()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IQueryResolver, ServiceProviderQueryResolver>()
            .BuildServiceProvider();
        var resolver = serviceProvider.GetService<IQueryResolver>()!;
        
        Assert.Throws<InvalidOperationException>(() => resolver.Resolve<TestQuery>());
    }

    [Fact]
    public void Query_args_handler_not_registered()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IQueryResolver, ServiceProviderQueryResolver>()
            .BuildServiceProvider();
        var resolver = serviceProvider.GetService<IQueryResolver>()!;
        
        Assert.Throws<InvalidOperationException>(() => resolver.Resolve<TestQuery, TestQueryArgs>());
    }

    [Fact]
    public void Query_handler_registered()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IQueryHandler<TestQuery>, TestQueryHandler>()
            .AddScoped<IQueryResolver, ServiceProviderQueryResolver>()
            .BuildServiceProvider();
        var resolver = serviceProvider.GetService<IQueryResolver>()!;
        
        var handler = resolver.Resolve<TestQuery>();
        Assert.NotNull(handler);
    }

    [Fact]
    public void Query_args_handler_registered()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IQueryHandler<TestQuery, TestQueryArgs>, TestQueryArgsHandler>()
            .AddScoped<IQueryResolver, ServiceProviderQueryResolver>()
            .BuildServiceProvider();
        var resolver = serviceProvider.GetService<IQueryResolver>()!;
        
        var handler = resolver.Resolve<TestQuery, TestQueryArgs>();
        Assert.NotNull(handler);
    }

    [Fact]
    public void Query_handler_registered_twice()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IQueryHandler<TestQuery>, TestQueryHandler>()
            .AddScoped<IQueryHandler<TestQuery>, TestQueryHandler>()
            .AddScoped<IQueryResolver, ServiceProviderQueryResolver>()
            .BuildServiceProvider();
        var resolver = serviceProvider.GetService<IQueryResolver>()!;
        
        Assert.Throws<InvalidOperationException>(() => resolver.Resolve<TestQuery>());
    }

    [Fact]
    public void Query_args_handler_registered_twice()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IQueryHandler<TestQuery, TestQueryArgs>, TestQueryArgsHandler>()
            .AddScoped<IQueryHandler<TestQuery, TestQueryArgs>, TestQueryArgsHandler>()
            .AddScoped<IQueryResolver, ServiceProviderQueryResolver>()
            .BuildServiceProvider();
        var resolver = serviceProvider.GetService<IQueryResolver>()!;
        
        Assert.Throws<InvalidOperationException>(() => resolver.Resolve<TestQuery, TestQueryArgs>());
    }

    [Fact]
    public void Query_handler_duplicate_registered()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IQueryHandler<TestQuery>, TestQueryHandler>()
            .AddScoped<IQueryHandler<TestQuery>, DuplicateQueryHandler>()
            .AddScoped<IQueryResolver, ServiceProviderQueryResolver>()
            .BuildServiceProvider();
        var resolver = serviceProvider.GetService<IQueryResolver>()!;
        
        Assert.Throws<InvalidOperationException>(() => resolver.Resolve<TestQuery>());
    }

    [Fact]
    public void Query_args_handler_duplicate_registered()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IQueryHandler<TestQuery, TestQueryArgs>, TestQueryArgsHandler>()
            .AddScoped<IQueryHandler<TestQuery, TestQueryArgs>, DuplicateQueryArgsHandler>()
            .AddScoped<IQueryResolver, ServiceProviderQueryResolver>()
            .BuildServiceProvider();
        var resolver = serviceProvider.GetService<IQueryResolver>()!;
        
        Assert.Throws<InvalidOperationException>(() => resolver.Resolve<TestQuery, TestQueryArgs>());
    }
}

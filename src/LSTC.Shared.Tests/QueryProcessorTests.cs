using Xunit;
using Microsoft.Extensions.DependencyInjection;
using LSTC.Shared.CQS.Queries;
using System.ComponentModel.DataAnnotations;

namespace LSTC.Shared.Tests;

public class QueryProcessorTests
{
    public class TestQuery : IQueryResults
    {
        public class Args : IQueryArgs
        {
            [Required, MinLength(3), MaxLength(100), RegularExpression(@"^[A-Z]+$")]
            public string? Name { get; set; }

            public Args(string? name)
            {
                Name = name;
            }
        }

        public class Handler : IQueryHandler<TestQuery>, IQueryHandler<TestQuery, Args>
        {
            public Task<TestQuery> ExecuteAsync()
            {
                return Task.FromResult(new TestQuery());
            }

            public Task<TestQuery> ExecuteAsync(Args args)
            {
                return Task.FromResult(new TestQuery());
            }
        }
    }

    [Fact]
    public void CommandProcessor_succeeds_validation_with_no_errors()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IQueryResolver, ServiceProviderQueryResolver>()
            .AddScoped<QueryProcessor>()
            .AddScoped<IQueryHandler<TestQuery, TestQuery.Args>, TestQuery.Handler>()
            .BuildServiceProvider();
        var processor = serviceProvider.GetService<QueryProcessor>()!;
        
        processor.ExecuteAsync<TestQuery, TestQuery.Args>(new TestQuery.Args("VALID")).Wait();
    }

    [Fact]
    public async Task CommandProcessor_fails_validation_with_multiple_errors()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IQueryResolver, ServiceProviderQueryResolver>()
            .AddScoped<QueryProcessor>()
            .AddScoped<IQueryHandler<TestQuery, TestQuery.Args>, TestQuery.Handler>()
            .BuildServiceProvider();
        var processor = serviceProvider.GetService<QueryProcessor>()!;
        
        var ex = await Assert.ThrowsAsync<AggregateException>(
            () => processor.ExecuteAsync<TestQuery, TestQuery.Args>(new TestQuery.Args("x"))
        );
        Assert.All(ex.InnerExceptions, innerEx => Assert.IsType<ValidationException>(innerEx));
    }

        [Fact]
    public async Task CommandProcessor_fails_validation_with_single_error()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IQueryResolver, ServiceProviderQueryResolver>()
            .AddScoped<QueryProcessor>()
            .AddScoped<IQueryHandler<TestQuery, TestQuery.Args>, TestQuery.Handler>()
            .BuildServiceProvider();
        var processor = serviceProvider.GetService<QueryProcessor>()!;
        

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => processor.ExecuteAsync<TestQuery, TestQuery.Args>(new TestQuery.Args("X"))
        );
    }
}

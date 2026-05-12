using LSTC.Shared.CQS.Commands;
using LSTC.Shared.CQS.Http;
using LSTC.Shared.CQS.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace LSTC.Shared.CQS.Tests;

public class HttpMapperTests
{
    public class TestCommand : ICommand
    {
        public string OrganisationId { get; set; }
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class TestCommandMap : HttpCommandMap<TestCommand>
    {
        public TestCommandMap()
        {
            Classify("Context", "Test");
            Classify("Resource", "TestResource");
            FromHeader(x => x.OrganisationId, "X-Organisation-Id");
            FromPath(x => x.Id);
            FromBody(x => x.Code);
            FromBody(x => x.Name);
        }
    }

    [Fact]
    public async void Can_map_commands()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["X-Organisation-Id"] = "Org123";
        httpContext.Request.Path = "/test/456";
        var json = @"{
            ""code"": ""Code123"",
            ""name"": ""Test Name""
        }";
        httpContext.Request.Body = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        httpContext.Request.ContentType = "application/json";

        var routeValues = new RouteValueDictionary { { "id", "456" } };

        var map = new TestCommandMap();
        var command = await map.CreateAsync(httpContext.Request, routeValues);

        Assert.NotNull(command);
        Assert.Equal("Org123", command.OrganisationId);
        Assert.Equal("456", command.Id);
        Assert.Equal("Code123", command.Code);
        Assert.Equal("Test Name", command.Name);
    }
    

    public class TestQueryArgs : IQueryArgs
    {
        public string OrganisationId { get; set; }
        public string Query { get; set; }
    }

    public class TestQueryArgsMap : HttpQueryArgsMap<TestQueryArgs>
    {
        public TestQueryArgsMap()
        {
            Classify("Context", "Test");
            Classify("Resource", "TestResource");
            FromHeader(x => x.OrganisationId, "X-Organisation-Id");
            FromQueryString(x => x.Query);
        }
    }

    [Fact]
    public async void Can_map_query_args()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["X-Organisation-Id"] = "Org123";
        httpContext.Request.QueryString = new QueryString("?query=TestQuery");

        var routeValues = new RouteValueDictionary();

        var map = new TestQueryArgsMap();
        var queryArgs = await map.CreateAsync(httpContext.Request, routeValues);

        Assert.NotNull(queryArgs);
        Assert.Equal("Org123", queryArgs.OrganisationId);
        Assert.Equal("TestQuery", queryArgs.Query);
    }
}

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Scalar.AspNetCore;

using LSTC.Shared.Http;
using LSTC.CheeseShop.CQS.Commands;
using LSTC.CheeseShop.Api.Services;
using LSTC.CheeseShop.Api.HealthChecks;
using LSTC.CheeseShop.CQS.Queries;

var builder = WebApplication.CreateBuilder(args);

// https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-9.0
builder.Services
    .AddSingleton(builder.Services)
    .AddHealthChecks()
    .AddCheck<CustomHealthCheck>("liveness", tags: ["live"])
    .AddCheck<CustomHealthCheck>("logs", tags: ["ready"])
    .AddCheck<CustomHealthCheck>("filesystem", tags: ["ready"])
    .AddCheck<CustomHealthCheck>("database", tags: ["ready"])
    .AddCheck<CustomHealthCheck>("eventbus", tags: ["ready"]);

// Add services to the container.
builder.Services
    .AddTransient<IUserService, UserService>()
    .AddCommandMaps(typeof(CreateProductCommandHttpMap).Assembly)
    .AddCommandHandlers(typeof(CreateProductCommandHttpMap).Assembly)
    .AddQueryMaps(typeof(ListProductsQueryHttpMap).Assembly)
    .AddQueryHandlers(typeof(ListProductsQueryHttpMap).Assembly);

builder.WebHost.ConfigureKestrel(options =>
{
    options.AllowSynchronousIO = true;
});

builder.Services.AddOpenApi(config =>
{
    config.AddDocumentTransformer((document, request, cancel) =>
    {
        document.Info.Title = "LSTC Cheese Shop API";
        document.Info.Description = "API for managing the LSTC Cheese Shop";
        document.Info.Version = "1.0.0";

        document.Components ??= new Microsoft.OpenApi.Models.OpenApiComponents();

        return Task.CompletedTask;
    });
});

var app = builder.Build();

async Task HealthCheckResponseWriter(HttpContext context, HealthReport report)
{
    var isVerbose = context.Request.Query.ContainsKey("verbose") && !string.Equals(context.Request.Query["verbose"], "false", StringComparison.InvariantCultureIgnoreCase);

    context.Response.ContentType = "text/plain";
    int statusCode;
    string message;
    switch (report.Status)
    {
        case HealthStatus.Healthy:
            statusCode = StatusCodes.Status200OK;
            message = "passed";
            break;
        case HealthStatus.Degraded:
            statusCode = StatusCodes.Status200OK;
            message = "degraded";
            break;
        default: // case HealthStatus.Unhealthy:
            statusCode = StatusCodes.Status503ServiceUnavailable;
            message = "failed";
            break;
    }
    context.Response.StatusCode = statusCode;
    using (var w = new StreamWriter(context.Response.Body))
    {
        if (isVerbose)
        {
            foreach (var e in report.Entries)
            {
                await w.WriteLineAsync(string.Format("[+] {0}: {1}", e.Key, e.Value.Status));
            }
        }
        await w.WriteLineAsync(
            string.Format(
                "{0} check {1}",
                context.Request.Path.ToString()[1..],
                message
            ));
        await w.FlushAsync();
    }
}

app.MapHealthChecks("/readyz", new HealthCheckOptions
{
    AllowCachingResponses = false,
    Predicate = healthCheck => healthCheck.Tags.Contains("ready"),
    ResponseWriter = HealthCheckResponseWriter
});

app.MapHealthChecks("/livez", new HealthCheckOptions
{
    AllowCachingResponses = false,
    Predicate = healthCheck => !healthCheck.Tags.Contains("ready"),
    ResponseWriter = HealthCheckResponseWriter
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/openapi");
}

app.MapEndpointsForCommands();
app.MapEndpointsForQueries();
app.Map("/*", () => Results.NotFound(new ApiResponse("NotFound")));

app.Run();

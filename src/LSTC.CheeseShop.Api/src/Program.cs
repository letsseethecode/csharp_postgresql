using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using LSTC.CheeseShop.Api.Services;
using LSTC.CheeseShop.Api.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-9.0
builder.Services.AddHealthChecks()
                .AddCheck<CustomHealthCheck>("liveness", tags: new[] { "live" })
                .AddCheck<CustomHealthCheck>("logs", tags: new[] { "ready" })
                .AddCheck<CustomHealthCheck>("filesystem", tags: new[] { "ready" })
                .AddCheck<CustomHealthCheck>("database", tags: new[] { "ready" })
                .AddCheck<CustomHealthCheck>("eventbus", tags: new[] { "ready" });
;

// Add services to the container.
builder.Services.AddTransient<IUserService, UserService>();
builder.WebHost.ConfigureKestrel(options =>
{
    options.AllowSynchronousIO = true;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

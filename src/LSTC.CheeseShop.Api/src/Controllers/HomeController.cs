using Microsoft.AspNetCore.Mvc;

using LSTC.CheeseShop.Api.Services;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LSTC.CheeseShop.Api.Controllers;

[ApiController]
[Route("/")]
public class HomeController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public HomeController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Indicates that the service has started and is alive - https://kubernetes.io/docs/reference/using-api/health-checks/
    /// </summary>
    /// <returns></returns>
    [HttpGet("livez")]
    public async Task Liveness([FromQuery] bool verbose = false)
    {
        var isVerbose = Request.Query.ContainsKey(nameof(verbose)) && verbose != false; // needs to handle '?verbose' and not just '?verbose=true'
        var responses = new string[] { };
        Response.ContentType = "text/plain";
        using (var w = new StreamWriter(Response.Body))
        {
            var rand = new Random();
            foreach (var r in responses)
            {
                await Task.Delay(rand.Next(500, 2500));
                if (isVerbose)
                {
                    await w.WriteLineAsync(r);
                    await w.FlushAsync();
                }
            }
            await Task.Delay(rand.Next(500, 2500));
            await w.WriteLineAsync("healthz check passed");
            await w.FlushAsync();
        }
    }

    /// <summary>
    /// Indicates that the service can process requests - https://kubernetes.io/docs/reference/using-api/health-checks/
    /// </summary>
    /// <returns></returns>
    [HttpGet("readyz")]
    public async Task Readiness([FromQuery] bool? verbose = null)
    {
        var isVerbose = Request.Query.ContainsKey(nameof(verbose)) && verbose != false; // needs to handle '?verbose' and not just '?verbose=true'
        var responses = new string[] {
            "[+]log: ok",
            "[+]postgres: ok",
            "[+]temporal: ok",
        };
        Response.ContentType = "text/plain";
        using (var w = new StreamWriter(Response.Body))
        {
            var rand = new Random();
            foreach (var r in responses)
            {
                await Task.Delay(rand.Next(500, 2500));
                if (isVerbose)
                {
                    await w.WriteLineAsync(r);
                    await w.FlushAsync();
                }
            }
            await Task.Delay(rand.Next(500, 2500));
            await w.WriteLineAsync("healthz check passed");
            await w.FlushAsync();
        }
    }

    /// <summary>
    /// Indicates that the service can process requests - https://kubernetes.io/docs/reference/using-api/health-checks/
    /// </summary>
    /// <returns></returns>
    [HttpGet("readyz/{service}")]
    public async Task ServiceReadiness(string service, [FromQuery] bool verbose = false)
    {
        var isVerbose = Request.Query.ContainsKey(nameof(verbose)) && verbose != false; // needs to handle '?verbose' and not just '?verbose=true'
        var responses = new string[] {
        };
        Response.ContentType = "text/plain";
        using (var w = new StreamWriter(Response.Body))
        {
            var rand = new Random();
            foreach (var r in responses)
            {
                await Task.Delay(rand.Next(500, 2500));
                if (isVerbose)
                {
                    await w.WriteLineAsync(r);
                    await w.FlushAsync();
                }
            }
            await Task.Delay(rand.Next(500, 2500));
            await w.WriteLineAsync("healthz check passed");
            await w.FlushAsync();
        }
    }
}

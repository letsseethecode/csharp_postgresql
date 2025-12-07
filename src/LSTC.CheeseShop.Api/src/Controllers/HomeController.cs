using Microsoft.AspNetCore.Mvc;

using LSTC.CheeseShop.Api.Services;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Text;

namespace LSTC.CheeseShop.Api.Controllers;

[ApiController]
[Route("/")]
public class HomeController : ControllerBase
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpDelete("echo")]
    [HttpGet("echo")]
    [HttpHead("echo")]
    [HttpPost("echo")]
    [HttpPatch("echo")]
    [HttpPut("echo")]
    public async Task<IActionResult> Echo()
    {
        using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
        {
            var body = await reader.ReadToEndAsync();
            var result = new
            {
                method = Request.Method.ToString(),
                path = Request.Path.Value,
                host = Request.Host.Value,
                headers = Request.Headers,
                query = Request.Query,
                body = body,
            };
            return Ok(result);
        }
    }
}

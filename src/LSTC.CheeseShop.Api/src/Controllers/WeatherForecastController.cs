using Microsoft.AspNetCore.Mvc;

using LSTC.CheeseShop.Api.Services;

namespace LSTC.CheeseShop.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IUserService _userService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
        _userService.Foo();
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public ActionResult<IEnumerable<WeatherForecast>> Get()
    {
        var results = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = 99,
            Summary = "Ok"
        })
        .ToArray();
        return Ok(results);
    }
}

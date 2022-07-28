using Microsoft.AspNetCore.Mvc;

namespace Example02.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;
    private readonly ILogger<WeatherController> _logger;

    public WeatherController(IWeatherService weatherService, ILogger<WeatherController> logger)
    {
        _weatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("{city}")]
    public async Task<IActionResult> GetWeatherInfo(string city, CancellationToken cancellationToken)
    {
        var info = await _weatherService.GetWeatherInfoAsync(city, cancellationToken);
        if (info is null) return NotFound($"Weather not found for city {city}");
        return Ok(info);
    }
}
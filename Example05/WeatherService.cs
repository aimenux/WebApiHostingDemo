using System.Text.Json.Serialization;

namespace Example05;

public interface IWeatherService
{
    Task<WeatherInfo> GetWeatherInfoAsync(string city, CancellationToken cancellationToken = default);
}

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<WeatherService> _logger;

    public WeatherService(HttpClient httpClient, IConfiguration configuration, ILogger<WeatherService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<WeatherInfo> GetWeatherInfoAsync(string city, CancellationToken cancellationToken)
    {
        var relativePath = _configuration.GetValue<string>("WeatherApi:RelativePath");
        var dto = await _httpClient.GetFromJsonAsync<WeatherDto>($"/{relativePath}/{city}", cancellationToken);
        if (dto is null) return null;
        return new WeatherInfo
        {
            Temperature = dto.Temperature,
            Description = dto.Description,
            Wind = dto.Wind
        };
    }

    internal class WeatherDto
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("temperature")]
        public string Temperature { get; set; }

        [JsonPropertyName("wind")]
        public string Wind { get; set; }
    }
}
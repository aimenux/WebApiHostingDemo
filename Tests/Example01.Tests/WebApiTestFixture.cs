using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Example01.Tests;

internal class WebApiTestFixture : WebApplicationFactory<Program>
{
    private readonly WeatherInfo _weatherInfo;

    public WebApiTestFixture(WeatherInfo weatherInfo)
    {
        _weatherInfo = weatherInfo;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.testing.json");
            configBuilder.AddJsonFile(configPath);
        });

        builder.ConfigureTestServices(services =>
        {
            var weatherService = Substitute.For<IWeatherService>();
            weatherService
                .GetWeatherInfoAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(_weatherInfo));
            services.AddScoped(_ => weatherService);
        });
    }
}
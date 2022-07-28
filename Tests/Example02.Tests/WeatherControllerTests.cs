using System.Net;
using FluentAssertions;

namespace Example02.Tests;

public class WeatherControllerTests
{
    [Fact]
    public async Task ShouldGetWeatherInfoReturnOk()
    {
        // arrange
        var weatherInfo = new WeatherInfo
        {
            Wind = "15 km/h",
            Temperature = "+38 °C",
            Description = "Sunny"
        };

        var fixture = new WebApiTestFixture(weatherInfo);

        var client = fixture.CreateClient();

        // act
        var response = await client.GetAsync("/weather/known-city");
        var responseBody = await response.Content.ReadAsStringAsync();

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseBody.Should().Be("{\"description\":\"Sunny\",\"temperature\":\"+38 °C\",\"wind\":\"15 km/h\"}");
    }

    [Fact]
    public async Task ShouldGetWeatherInfoReturnNotFound()
    {
        // arrange
        var fixture = new WebApiTestFixture(null);

        var client = fixture.CreateClient();

        // act
        var response = await client.GetAsync("/weather/unknown-city");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
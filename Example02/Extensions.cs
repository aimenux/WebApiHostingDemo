namespace Example02;

public static class Extensions
{
    public static void AddWeatherDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient<IWeatherService, WeatherService>((provider, client) =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var url = configuration.GetValue<string>("WeatherApi:BaseUrl");
            client.Timeout = TimeSpan.FromSeconds(5);
            client.BaseAddress = new Uri(url);
        });
    }
}
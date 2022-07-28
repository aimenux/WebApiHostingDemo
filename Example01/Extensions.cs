using Serilog;
using Serilog.Debugging;

namespace Example01;

public static class Extensions
{
    public static void AddWeatherDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient<IWeatherService, WeatherService>(client =>
        {
            var url = builder.Configuration.GetValue<string>("WeatherApi:BaseUrl");
            client.Timeout = TimeSpan.FromSeconds(5);
            client.BaseAddress = new Uri(url);
        });
    }

    public static void AddSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
        {
            SelfLog.Enable(Console.Error);

            loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration)
                .Enrich.FromLogContext();
        });
    }
}
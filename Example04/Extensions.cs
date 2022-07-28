using Serilog;
using Serilog.Debugging;

namespace Example04;

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

    public static WebApplication Build<TStartup>(this WebApplicationBuilder builder) where TStartup : IStartup
    {
        var startup = Activator.CreateInstance(typeof(TStartup), new object[] { builder.Configuration });
        if (startup == null)
        {
            throw new InvalidOperationException("Could not instantiate Startup class");
        }

        var configureServices = typeof(TStartup).GetMethod(nameof(IStartup.ConfigureServices));
        if (configureServices == null)
        {
            throw new InvalidOperationException($"Could not find {nameof(IStartup.ConfigureServices)} on Startup class");
        }

        configureServices.Invoke(startup, new object[] { builder.Services });

        var app = builder.Build();

        var configure = typeof(TStartup).GetMethod(nameof(IStartup.Configure));
        if (configure == null)
        {
            throw new InvalidOperationException($"Could not find {nameof(IStartup.Configure)} on Startup class");
        }

        configure.Invoke(startup, new object[] { app, app.Environment });
        return app;
    }

    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
        {
            SelfLog.Enable(Console.Error);

            loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration)
                .Enrich.FromLogContext();
        });

        return builder;
    }
}
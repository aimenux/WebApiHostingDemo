using Example05;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddWeatherDependencies();
builder.AddSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options => options.DisplayRequestDuration());
}

app.UseHttpsRedirection();

app.MapGet("/weather/{city}", async (string city, IWeatherService weatherService, CancellationToken cancellationToken) =>
{
    var info = await weatherService.GetWeatherInfoAsync(city, cancellationToken);
    if (info is null) return Results.NotFound($"Weather not found for city {city}");
    return Results.Ok(info);
});

app.Run();

//public partial class Program { }
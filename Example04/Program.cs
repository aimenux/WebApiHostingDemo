using Example04;

var builder = WebApplication.CreateBuilder(args).AddSerilog();
var app = builder.Build<Startup>();
app.Run();
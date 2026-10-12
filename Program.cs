using System.Text.Json.Serialization;
using TaskBoard.Api.Domain;
using TaskBoard.Api.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false)));

var app = builder.Build();

app.MapControllers();

app.UseMiddleware<MaintenanceMiddleware>();
app.UseMiddleware<IdMiddleware>();
app.UseMiddleware<RequestTimingMiddleware>();


app.Run();
using System.Text.Json.Serialization;
using TaskBoard.Api.Domain;
using TaskBoard.Api.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();

app.UseMiddleware<MaintenanceMiddleware>();
app.UseMiddleware<IdMiddleware>();
app.UseMiddleware<RequestTimingMiddleware>();


app.Run();
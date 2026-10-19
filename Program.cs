using System.Text.Json.Serialization;

using TaskBoard.Api.Infrastructure;
using TaskBoard.Api.Services;
using TaskBoard.Api.Domain;
using TaskBoard.Api.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false)));

builder.Services.AddSingleton<IClock, SystemClock>();
builder.Services.AddSingleton<ITaskRepository, InMemoryTaskRepository>();
builder.Services.AddScoped<TaskService>();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<DomainExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();

app.MapControllers();

app.Run();
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

using TaskBoard.Api.Infrastructure;
using TaskBoard.Api.Services;
using TaskBoard.Api.Domain;
using TaskBoard.Api.Http;
using TaskBoard.Api.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false)));

builder.Services.AddSingleton<IClock, SystemClock>();
builder.Services.AddScoped<ITaskRepository, EfTaskRepository>();
builder.Services.AddScoped<IProjectRepository, EfProjectRepository>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<ProjectService>();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<DomainExceptionHandler>();
builder.Services.AddExceptionHandler<ConcurrencyExceptionHandler>();

builder.Services.AddDbContext<TaskBoardDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("TaskBoard")));

var app = builder.Build();

app.UseExceptionHandler();

app.MapControllers();

app.Run();
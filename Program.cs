using System.Text.Json.Serialization;
using TaskBoard.Api.Domain;
using TaskBoard.Api.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();

app.UseMiddleware<RequestTimingMiddleware>();
app.UseMiddleware<MaintenanceMiddleware>();

var tasks = new List<TaskItem>();

TaskItem? FindTask(Guid id) => tasks.FirstOrDefault(t => t.Id == id);

var api = app.MapGroup("/api/tasks");

api.MapGet("/", () => tasks);

api.MapGet("/{id:guid}", (Guid id) =>
{
   TaskItem? task = FindTask(id);
   return task == null ? Results.NotFound() : Results.Ok(task); 
});

api.MapPost("/", (CreateTaskRequest request) =>
{
    var task = new TaskItem
    {
        Id = Guid.CreateVersion7(),
        Title = request.Title,
        Status = TaskItemStatus.Todo,
        CreatedAt = DateTime.Now
    };
    tasks.Add(task);
    return Results.Created($"/api/tasks/{task.Id}", task);
});

api.MapPut("/{id:guid}/title", (Guid id, RenameTaskRequest request) =>
{
    var task = FindTask(id);
    if (task is null)
    {
        return Results.NotFound();
    }
    task.Title = request.Title;
    return Results.Ok(task);
});

api.MapPost("/{id:guid}/start", (Guid id) =>
{
   TaskItem task = FindTask(id);
   if (task is null)
    {
        return Results.NotFound();
    } 
    task.Status = TaskItemStatus.InProgress;
    return Results.Ok(task);
});

api.MapPost("/{id:guid}/complete", (Guid id) =>
{
    TaskItem task = FindTask(id);
    if (task is null)
    {
        return Results.NotFound();
    }
    task.Status = TaskItemStatus.Done;
    task.CompletedAt = DateTime.Now;
    return Results.Ok(task);
});

api.MapPost("/{id:guid}/reopen", (Guid id) =>
{
    var task = FindTask(id);
    if (task is null)
    {
        return Results.NotFound();
    }

    task.Status = TaskItemStatus.Todo;
    task.CompletedAt = null;
    return Results.Ok(task);
});

app.MapDelete("/{id:guid}", (Guid id) =>
{
    TaskItem task = FindTask(id);
    if (task is null)
    {
        return Results.NotFound();
    }
    tasks.Remove(task);
    return Results.NoContent();
});

app.MapGet("/", (TaskItemStatus? status) =>
{
    if (Enum.IsDefined(status.Value)) 
    {
        return Results.BadRequest("Nieznany status.");
    }
    var res = tasks.Where(p => p.Status == status).ToList();
    return res.Count() == 0 ? Results.Ok(tasks) : Results.Ok(res);
});



app.Run();

record CreateTaskRequest(string Title);
record RenameTaskRequest(string Title);
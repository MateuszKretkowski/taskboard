using TaskBoard.Api.Domain;
using TaskBoard.Api.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace TaskBoard.Api.Controllers;

[ApiController]
[Route("/api/tasks")]
public class TasksController : ControllerBase
{
    // temp
    public static readonly List<TaskItem> tasks = [];

    [HttpGet]
    public IReadOnlyList<TaskResponse> GetAll()
    {
        return tasks.Select(t => TaskResponse.From(t)).ToList();
    }

    [HttpGet("{id:guid}")]
    public ActionResult<TaskResponse> GetById(Guid id)
    {
        return ToResponse(Find(id));
    }

    [HttpPost]
    public ActionResult<TaskResponse> Create(CreateTaskRequest request)
    {
        TaskItem task = new TaskItem{
            Id = Guid.CreateVersion7(),
            Title = request.Title,
            Status = TaskItemStatus.Todo,
            CreatedAt = DateTime.Now,
        };

        tasks.Add(task);

        return CreatedAtAction(nameof(GetById), new { id = task.Id }, TaskResponse.From(task));
    }

    [HttpPut("{id:guid}/title")]
    public ActionResult<TaskResponse> Rename(Guid id, RenameTaskRequest request)
    {
        TaskItem? task = Find(id);
        if (task is null)
        {
            return NotFound();
        }
        task.Title = request.Title;
        return Ok(TaskResponse.From(task));
    }

    [HttpPut("{id:guid}/start")]
    public ActionResult<TaskResponse> Start(Guid id)
    {
        TaskItem? task = Find(id);
        if (task is null)
        {
            return NotFound();
        }
        task.Status = TaskItemStatus.InProgress;
        return Ok(TaskResponse.From(task));
    }

    [HttpPut("{id:guid}/complete")]
    public ActionResult<TaskResponse> Complete(Guid id)
    {
        TaskItem task = Find(id);
        if (task is null)
        {
            return NotFound();
        }
        task.Status = TaskItemStatus.Done;
        task.CompletedAt = DateTime.Now;
        return Ok(TaskResponse.From(task));
    }

    [HttpPut("{id:guid}/reopen")]
    public ActionResult<TaskResponse> Reopen(Guid id)
    {
        TaskItem task = Find(id);
        if (task is null)
        {
            return NotFound();
        }
        task.Status = TaskItemStatus.InProgress;
        task.CompletedAt = null;
        return Ok(TaskResponse.From(task));
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        TaskItem task = Find(id);
        if (task is null)
        {
            return NotFound();
        }
        tasks.Remove(task);
        return NoContent();
    }

    private static TaskItem? Find(Guid id)
    {
        return tasks.FirstOrDefault(t => t.Id == id);
    }

    private ActionResult<TaskResponse> ToResponse(TaskItem task)
    {
        if (task is null)
        {
            return NotFound();
        }

        return TaskResponse.From(task);
    }
}
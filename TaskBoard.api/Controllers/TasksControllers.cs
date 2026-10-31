using TaskBoard.Api.Domain;
using TaskBoard.Api.Contracts;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Api.Services;
using TaskBoard.Api.Http;

namespace TaskBoard.Api.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly TaskService _service;

    public TasksController(TaskService service)
    {
        this._service = service;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TaskResponse>> GetById(Guid id)
    {
        var task = await _service.GetByIdAsync(User.GetUserId(), id);
        return ToResponse(task);
    }

    [HttpPut("{id:guid}/title")]
    public async Task<ActionResult<TaskResponse>> Rename(Guid id, RenameTaskRequest request)
    {
        var task = await _service.RenameAsync(User.GetUserId(), id, request.Title);
        return ToResponse(task);
    }

    [HttpPut("{id:guid}/start")]
    public async Task<ActionResult<TaskResponse>> Start(Guid id)
    {
        var task = await _service.StartAsync(User.GetUserId(), id);
        return ToResponse(task);
    }

    [HttpPut("{id:guid}/complete")]
    public async Task<ActionResult<TaskResponse>> Complete(Guid id)
    {
        var task = await _service.CompleteAsync(User.GetUserId(), id);
        return ToResponse(task);
    }

    [HttpPut("{id:guid}/reopen")]
    public async Task<ActionResult<TaskResponse>> Reopen(Guid id)
    {
        var task = await _service.ReopenAsync(User.GetUserId(), id);
        return ToResponse(task);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        bool deleted = await _service.DeleteAsync(User.GetUserId(), id);
        return deleted ? NoContent() : NotFound();
    }

    private ActionResult<TaskResponse> ToResponse(TaskItem? task)
    {
        if (task is null)
        {
            return NotFound();
        }

        return TaskResponse.From(task);
    }
}
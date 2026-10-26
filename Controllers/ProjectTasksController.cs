namespace TaskBoard.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using TaskBoard.Api.Contracts;
using TaskBoard.Api.Domain;
using TaskBoard.Api.Http;
using TaskBoard.Api.Services;

[Controller]
[Route("api/projects/{id:guid}/tasks")]
public class ProjectTasksController : ControllerBase
{
    private readonly TaskService _service;

    public ProjectTasksController(TaskService service)
    {
        this._service = service;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<TaskResponse>?>> Search(Guid id, [FromQuery] TaskListQuery query)
    {
        PagedResult<TaskItem>? pagedResult = await _service.SearchAsync(User.GetUserId(), id, query.status, query.page, query.pageSize);
        if (pagedResult is null)
        {
            return NotFound();
        }
        return PagedResponse<TaskResponse>.From(pagedResult, TaskResponse.From);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponse>> Create(Guid id, [FromQuery] CreateProjectRequest request)
    {
        var task = await _service.CreateAsync(User.GetUserId(), id, request.name);
        if (task is null)
        {
            return NotFound();
        }
        return CreatedAtAction(nameof(TasksController.GetById), "Tasks", new { Id = task.Id }, task);
    }
} 

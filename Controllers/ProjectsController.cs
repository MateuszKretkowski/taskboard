using Microsoft.AspNetCore.Mvc;
using TaskBoard.Api.Contracts;
using TaskBoard.Api.Domain;
using TaskBoard.Api.Services;

namespace TaskBoard.Api.Controllers;

[ApiController]
[Route("/api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly ProjectService _service;
    
    public ProjectsController(ProjectService service)
    {
        this._service = service;
    }

    [HttpGet]
    public async Task<IReadOnlyList<ProjectSummaryResponse>> GetAll()
    {
        var summaries = await _service.GetSummariesAsync();
        return summaries.Select(ProjectSummaryResponse.From).ToList();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProjectResponse>> GetById(Guid id)
    {
        var project = await _service.GetByIdAsync(id);
        return ToResponse(project);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectResponse>> Create(CreateProjectRequest request)
    {
        var project = await _service.CreateAsync(request.name);
        return CreatedAtAction(nameof(GetById), new {id = project.Id}, ProjectResponse.From(project));
    }

    [HttpPut("{id:guid}/name")]
    public async Task<ActionResult<ProjectResponse>> Rename(Guid id, RenameProjectReqeust request)
    {
        var project = await _service.RenameAsync(id, request.name);
        return ToResponse(project);
    }

    [HttpPost("{id:guid}/archive")]
    public async Task<ActionResult<ProjectResponse>> Archive(Guid id)
    {
        var project = await _service.ArchiveAsync(id);
        return ToResponse(project);
    }

    [HttpPost("{id:guid}/restore")]
    public async Task<ActionResult<ProjectResponse>> Restore(Guid id)
    {
        var project = await _service.RestoreAsync(id);
        return ToResponse(project);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        bool deleted = await _service.DeleteAsync(id);
        return deleted == true ? NoContent() : NotFound();
    }

    private ActionResult<ProjectResponse> ToResponse(Project? project)
    {
        if (project is null)
        {
            return NotFound();
        }

        return ProjectResponse.From(project);
    }
}
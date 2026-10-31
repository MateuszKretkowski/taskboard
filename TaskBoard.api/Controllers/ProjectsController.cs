using Microsoft.AspNetCore.Mvc;
using TaskBoard.Api.Contracts;
using TaskBoard.Api.Domain;
using TaskBoard.Api.Http;
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

    [HttpPost("add-member")]
    public async Task<ActionResult<ProjectResponse>> AddMember(ManipulateMemberRequest request)
    {
        Project? project = await _service.AddMember(User.GetUserId(), request.projectId, request.userId);
        if (project is null)
        {
            return NotFound();
        }
        return Ok(ProjectResponse.From(project));
    }

    [HttpPost("remove-member")]
    public async Task<IActionResult> RemoveMember(ManipulateMemberRequest request)
    {
        Project? project = await _service.RemoveMember(User.GetUserId(), request.projectId, request.userId);
        if (project is null)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpGet]
    public async Task<IReadOnlyList<ProjectSummaryResponse>> GetAll()
    {
        var summaries = await _service.GetSummariesAsync(User.GetUserId());
        return summaries.Select(ProjectSummaryResponse.From).ToList();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProjectResponse>> GetById(Guid id)
    {
        var project = await _service.GetByIdAsync(User.GetUserId(), id);
        return ToResponse(project);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectResponse>> Create(CreateProjectRequest request)
    {
        var project = await _service.CreateAsync(User.GetUserId(), request.name);
        return CreatedAtAction(nameof(GetById), new {id = project.Id}, ProjectResponse.From(project));
    }

    [HttpPut("{id:guid}/name")]
    public async Task<ActionResult<ProjectResponse>> Rename(Guid id, RenameProjectReqeust request)
    {
        var project = await _service.RenameAsync(User.GetUserId(), id, request.name);
        return ToResponse(project);
    }

    [HttpPost("{id:guid}/archive")]
    public async Task<ActionResult<ProjectResponse>> Archive(Guid id)
    {
        var project = await _service.ArchiveAsync(User.GetUserId(), id);
        return ToResponse(project);
    }

    [HttpPost("{id:guid}/restore")]
    public async Task<ActionResult<ProjectResponse>> Restore(Guid id)
    {
        var project = await _service.RestoreAsync(User.GetUserId(), id);
        return ToResponse(project);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        bool deleted = await _service.DeleteAsync(User.GetUserId(), id);
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
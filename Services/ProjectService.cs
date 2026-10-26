using TaskBoard.Api.Domain;

namespace TaskBoard.Api.Services;

public class ProjectService
{
    private readonly IProjectRepository _projects;
    private readonly IClock _clock;
    private readonly ILogger<ProjectService> _logger;

    public ProjectService(IProjectRepository projects, IClock clock, ILogger<ProjectService> logger)
    {
        this._projects = projects;
        this._clock = clock;
        this._logger = logger;
    }

    public Task<IReadOnlyList<ProjectSummary>> GetSummariesAsync(Guid ownerId) => _projects.GetSummariesAsync(ownerId);
    
    public Task<Project?> GetByIdAsync(Guid ownerId, Guid id) => GetOwnedAsync(ownerId, id);

    public async Task<Project> CreateAsync(Guid ownerId, string name)
    {
        var project = new Project(ownerId, name, _clock.UtcNow);
        await _projects.AddAsync(project);
        await _projects.SaveChangesAsync();
        _logger.LogInformation("Utworzono projekt {ProjektId}", project.Id);
        return project;
    }

    public Task<Project?> RenameAsync(Guid ownerId, Guid id, string name) => ModifyAsync(ownerId, id, t =>
    {
        t.Rename(name);
    });

    public Task<Project?> ArchiveAsync(Guid ownerId, Guid id) => ModifyAsync(ownerId, id, t =>
    {
        t.Archive();
    });

    public Task<Project?> RestoreAsync(Guid ownerId, Guid id) => ModifyAsync(ownerId, id, t =>
    {
        t.Restore();
    });

    public async Task<bool> DeleteAsync(Guid ownerId, Guid id)
    {
        var project = await GetOwnedAsync(ownerId, id);
        if (project is null)
        {
            return false;
        }
        await _projects.RemoveAsync(project);
        await _projects.SaveChangesAsync();
        _logger.LogInformation("Usunieto projekt {ProjectId}", id);
        return true;
    }

    private async Task<Project?> GetOwnedAsync(Guid ownerId, Guid id)
    {
        Project? project = await _projects.GetByIdAsync(id);
        if (project is null || !project.IsOwnedBy(ownerId))
        {
            return null;
        }
        return project;
    }

    public async Task<Project?> ModifyAsync(Guid ownerId, Guid id, Action<Project> change)
    {
        var project = await GetOwnedAsync(ownerId, id);
        if (project is null)
        {
            return null;
        }
        change(project);
        await _projects.SaveChangesAsync();
        return project;
    }

}
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

    public Task<IReadOnlyList<ProjectSummary>> GetSummariesAsync() => _projects.GetSummariesAsync();
    
    public Task<Project?> GetByIdAsync(Guid id) => _projects.GetByIdAsync(id);

    public async Task<Project> CreateAsync(string name)
    {
        var project = new Project(name, _clock.UtcNow);
        await _projects.AddAsync(project);
        await _projects.SaveChangesAsync();
        _logger.LogInformation("Utworzono projekt {ProjektId}", project.Id);
        return project;
    }

    public Task<Project?> RenameAsync(Guid id, string name) => ModifyAsync(id, t =>
    {
        t.Rename(name);
    });

    public Task<Project?> ArchiveAsync(Guid id) => ModifyAsync(id, t =>
    {
        t.Archive();
    });

    public Task<Project?> RestoreAsync(Guid id) => ModifyAsync(id, t =>
    {
        t.Restore();
    });

    public async Task<bool> DeleteAsync(Guid id)
    {
        var project = await GetByIdAsync(id);
        if (project is null)
        {
            return false;
        }
        await _projects.RemoveAsync(project);
        await _projects.SaveChangesAsync();
        _logger.LogInformation("Usunieto projekt {ProjectId}", id);
        return true;
    }

    public async Task<Project?> ModifyAsync(Guid id, Action<Project> change)
    {
        var project = await GetByIdAsync(id);
        if (project is null)
        {
            return null;
        }
        change(project);
        await _projects.SaveChangesAsync();
        return project;
    }
}
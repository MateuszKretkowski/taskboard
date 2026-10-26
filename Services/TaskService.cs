using System.ComponentModel.DataAnnotations;
using TaskBoard.Api.Domain;

namespace TaskBoard.Api.Services;

public class TaskService
{
    private readonly ITaskRepository _repository;
    private readonly IProjectRepository _projects;
    private readonly IClock _clock;
    private readonly ILogger<TaskService> _logger;

    public TaskService(ITaskRepository repository, IProjectRepository projects, IClock clock, ILogger<TaskService> logger)
    {
        this._repository = repository;
        this._clock = clock;
        this._logger = logger;
        this._projects = projects;
    }

    public Task<TaskItem?> GetByIdAsync(Guid userId, Guid id) => GetOwnedAsync(userId, id);

    public async Task<PagedResult<TaskItem>?> SearchAsync(Guid userId, Guid projectId, TaskItemStatus? status, int page, int pageSize)
    {
        var project = await GetOwnedProjectAsync(userId, projectId);
        if (project is null)
        {
            return null;
        }
        return await _repository.SearchAsync(projectId, status, page, pageSize);
    }

    public async Task<TaskItem?> CreateAsync(Guid userId, Guid projectId, string title)
    {
        var project = await GetOwnedProjectAsync(userId, projectId);
        if (project is null || !project.IsOwnedBy(userId))
        {
            return null;
        }
        var task = project.CreateTask(title, _clock.UtcNow);
        await _repository.AddTaskAsync(task);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Utworzono zadanie {taskId} w projekcie {projectId}", task.Id, projectId);
        return task;
    }

    public Task<TaskItem?> RenameAsync(Guid userId, Guid id, string title) => 
        ModifyAsync(userId, id, task => task.Rename(title));

    public Task<TaskItem?> StartAsync(Guid userId, Guid id) => 
        ModifyAsync(userId, id, task => task.Start());

    public Task<TaskItem?> CompleteAsync(Guid userId, Guid id) => 
        ModifyAsync(userId, id, task => task.Complete(_clock.UtcNow));

    public Task<TaskItem?> ReopenAsync(Guid userId, Guid id) => 
        ModifyAsync(userId, id, task => task.Reopen());
    
    public async Task<bool> DeleteAsync(Guid userId, Guid id)
    {
        var task = await GetOwnedAsync(userId, id);
        if (task is null)
        {
            return false;
        }
        await _repository.RemoveAsync(task);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Usunieto zadanie: {TaskId}", task.Id);
        return true;
    }

    private async Task<TaskItem?> ModifyAsync(Guid userId, Guid id, Action<TaskItem> change)
    {
        var task = await GetOwnedAsync(userId, id);
        if (task is null)
        {
            return null;
        }

        change(task);

        await _repository.SaveChangesAsync();

        return task;
    }

    private async Task<Project?> GetOwnedProjectAsync(Guid userId, Guid id)
    {
        Project? project = await _projects.GetByIdAsync(id);
        if (project is null || project.IsOwnedBy(userId))
        {
            return null;
        }
        return project;
    }

    private async Task<TaskItem?> GetOwnedAsync(Guid userId, Guid id)
    {
        TaskItem? task = await _repository.GetByIdAsync(id);
        if (task is null)
        {
            return null;
        }
        var project = await GetOwnedProjectAsync(userId, task.ProjectId);
        return project is null ? null : task;
    }
}
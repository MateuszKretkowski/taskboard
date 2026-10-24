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

    public Task<TaskItem?> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);

    public async Task<PagedResult<TaskItem>?> SearchAsync(Guid projectId, TaskItemStatus? status, int page, int pageSize)
    {
        var project = await _projects.GetByIdAsync(projectId);
        if (project is null)
        {
            return null;
        }
        return await _repository.SearchAsync(projectId, status, page, pageSize);
    }

    public async Task<TaskItem?> CreateAsync(Guid projectId, string title)
    {
        var project = await _projects.GetByIdAsync(projectId);
        if (project is null)
        {
            return null;
        }
        var task = project.CreateTask(title, _clock.UtcNow);
        await _repository.AddTaskAsync(task);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Utworzono zadanie {taskId} w projekcie {projectId}", task.Id, projectId);
        return task;
    }

    public Task<TaskItem?> RenameAsync(Guid id, string title) => 
        ModifyAsync(id, task => task.Rename(title));

    public Task<TaskItem?> StartAsync(Guid id) => 
        ModifyAsync(id, task => task.Start());

    public Task<TaskItem?> CompleteAsync(Guid id) => 
        ModifyAsync(id, task => task.Complete(_clock.UtcNow));

    public Task<TaskItem?> ReopenAsync(Guid id) => 
        ModifyAsync(id, task => task.Reopen());
    
    public async Task<bool> DeleteAsync(Guid id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task is null)
        {
            return false;
        }
        await _repository.RemoveAsync(task);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Usunieto zadanie: {TaskId}", task.Id);
        return true;
    }

    private async Task<TaskItem?> ModifyAsync(Guid id, Action<TaskItem> change)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task is null)
        {
            return null;
        }

        change(task);

        await _repository.SaveChangesAsync();

        return task;
    }
}
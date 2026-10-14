using TaskBoard.Api.Domain;

namespace TaskBoard.Api.Services;

public class TaskService
{
    private readonly ITaskRepository _repository;
    private readonly IClock _clock;

    public TaskService(ITaskRepository repository, IClock clock)
    {
        this._repository = repository;
        this._clock = clock;
    }

    public Task<IReadOnlyList<TaskItem>> GetAllAsync() => _repository.GetAllAsync();

    public Task<TaskItem?> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);

    public async Task<TaskItem> CreateAsync(string title)
    {
        TaskItem task = new TaskItem(title, _clock.UtcNow);
        await _repository.AddTaskAsync(task);
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
        return task;
    }
}
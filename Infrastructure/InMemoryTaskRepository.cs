using TaskBoard.Api.Services;
using TaskBoard.Api.Domain;

namespace TaskBoard.Api.Infrastructure;

public class InMemoryTaskRepository : ITaskRepository
{
    private readonly List<TaskItem> tasks = [];
    private readonly Lock _lock = new();

    public Task AddTaskAsync(TaskItem task)
    {
        lock (_lock)
        {
            tasks.Add(task);
        }

        return Task.CompletedTask;
    }

    public Task<TaskItem?> GetByIdAsync(Guid id)
    {
        lock (_lock)
        {
            return Task.FromResult(tasks.FirstOrDefault(t => t.Id == id));
        }
    }

    public Task<IReadOnlyList<TaskItem>> GetAllAsync()
    {
        lock (_lock)
        {
            return Task.FromResult<IReadOnlyList<TaskItem>>(tasks.ToList());
        }
    }

    public Task RemoveAsync(TaskItem task)
    {
        lock (_lock)
        {
            tasks.Remove(task);
            return Task.CompletedTask;
        }
    }
}
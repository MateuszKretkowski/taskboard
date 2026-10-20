using TaskBoard.Api.Domain;

namespace TaskBoard.Api.Services;

public interface ITaskRepository
{
    Task AddTaskAsync(TaskItem task);
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<TaskItem>> GetAllAsync();
    Task RemoveAsync(TaskItem task);
    Task SaveChangesAsync();
}
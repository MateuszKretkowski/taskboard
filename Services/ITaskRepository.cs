using TaskBoard.Api.Domain;

namespace TaskBoard.Api.Services;

public interface ITaskRepository
{
    Task AddTaskAsync(TaskItem task);
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task RemoveAsync(TaskItem task);
    Task SaveChangesAsync();
    Task<PagedResult<TaskItem>> SearchAsync(Guid projectId, TaskItemStatus? status, int page, int pageSize);
}
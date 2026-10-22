using TaskBoard.Api.Domain;

namespace TaskBoard.Api.Services;

public interface IProjectRepository
{
    Task AddAsync(Project project);
    Task<Project?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Project>> GetSummariesAsync();
    Task RemoveAsync(Project project);
    Task SaveChangesAsync();
}
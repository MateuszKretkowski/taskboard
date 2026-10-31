using TaskBoard.Api.Domain;

namespace TaskBoard.Api.Services;

public interface IProjectRepository
{
    Task AddAsync(Project project);
    Task<Project?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<ProjectSummary>> GetSummariesAsync(Guid ownerId);
    Task RemoveAsync(Project project);
    Task SaveChangesAsync();
}
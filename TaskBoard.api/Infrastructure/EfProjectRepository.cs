namespace TaskBoard.Api.Infrastructure;

using Microsoft.EntityFrameworkCore;
using TaskBoard.Api.Domain;
using TaskBoard.Api.Infrastructure.Persistence;
using TaskBoard.Api.Services;

public class EfProjectRepository : IProjectRepository 
{
    private readonly TaskBoardDbContext _db;

    public EfProjectRepository(TaskBoardDbContext db)
    {
        this._db = db;
    }

    public Task AddAsync(Project project)
    {
        _db.Projects.Add(project);
        return Task.CompletedTask;
    }

    public async Task<Project?> GetByIdAsync(Guid id)
    {
        return await _db.Projects.Include(p => p.Members).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IReadOnlyList<ProjectSummary>> GetSummariesAsync(Guid ownerId)
    {
        return await _db.Projects.Join(_db.ProjectMembers, p => p.Id, pm => pm.ProjectId, (p, pm) => new {p, pm}).Where(pm => pm.pm.UserId == ownerId).OrderBy(p => p.p.CreatedAt).Select(p => new ProjectSummary(p.p.Id, p.p.Name, p.p.IsArchived, _db.Tasks.Where(t => t.ProjectId == p.p.Id && t.Status != TaskItemStatus.Done).Count(), _db.Tasks.Where(t => t.ProjectId == p.p.Id && t.Status == TaskItemStatus.Done).Count())).ToListAsync();
    }

    public Task RemoveAsync(Project project)
    {
        _db.Projects.Remove(project);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
using TaskBoard.Api.Services;
using TaskBoard.Api.Domain;
using TaskBoard.Api.Infrastructure.Persistence;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace TaskBoard.Api.Infrastructure;

public class EfTaskRepository : ITaskRepository
{
    private readonly TaskBoardDbContext _db;
    public EfTaskRepository(TaskBoardDbContext db)
    {
        this._db = db;
    }

    public Task AddTaskAsync(TaskItem task)
    {
        _db.Add(task);
        return Task.CompletedTask;
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        return await _db.Tasks.FindAsync(id);
    }

    public async Task<IReadOnlyList<TaskItem>> GetAllAsync()
    {
        return await _db.Tasks.OrderBy(t => t.CreatedAt).ToListAsync();
    }

    public Task RemoveAsync(TaskItem task)
    {
        _db.Remove(task);
        return Task.CompletedTask;
    }

    public async Task<PagedResult<TaskItem>> SearchAsync(Guid projectId, TaskItemStatus? status, int page, int pageSize)
    {
        IQueryable<TaskItem> query = _db.Tasks.AsNoTracking().Where(t => t.ProjectId == projectId);

        if (status is not null)
        {
            query = query.Where(t => t.Status == status);
        }

        int totalCount = await query.CountAsync();

        var items = await query.OrderBy(t => t.CreatedAt).ThenBy(t => t.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<TaskItem>(items, page, pageSize, totalCount);
    }

    public Task SaveChangesAsync()
    {
        return _db.SaveChangesAsync();
    }
}
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

    public Task SaveChangesAsync()
    {
        return _db.SaveChangesAsync();
    }
}
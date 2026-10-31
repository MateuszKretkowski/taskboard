namespace TaskBoard.Api.Infrastructure;

using Microsoft.EntityFrameworkCore;
using TaskBoard.Api.Domain;
using TaskBoard.Api.Infrastructure.Persistence;
using TaskBoard.Api.Services;

public class EfUserRepository : IUserRepository
{
    private readonly TaskBoardDbContext _db;

    public EfUserRepository(TaskBoardDbContext dbContext)
    {
        this._db = dbContext;
    }

    public Task AddAsync(User user)
    {
        _db.Add(user);
        return Task.CompletedTask;
    }
    
    public async Task<User?> GetByIdAsync(Guid id)
    {
        User? user = await _db.Users.FindAsync(id);
        if (user is null)
        {
            return null;
        }
        return user;
    }

    public async Task<User?> GetByEmailAsync(Email email)
    {
        User? user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user is null)
        {
            return null;
        }
        return user;
    }

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
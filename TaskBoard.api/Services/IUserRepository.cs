using TaskBoard.Api.Domain;

namespace TaskBoard.Api.Services;

public interface IUserRepository
{
    public Task AddAsync(User user);
    public Task<User?> GetByIdAsync(Guid id);
    public Task<User?> GetByEmailAsync(Email email);
    public Task SaveChangesAsync();
}
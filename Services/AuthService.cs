namespace TaskBoard.Api.Services;

using TaskBoard.Api.Domain;

public class AuthService
{
    public const int MinPasswordLength = 8;
    public const int MaxPasswordLength = 128;

    private readonly IUserRepository _users;
    private readonly IClock _clock;
    private readonly ILogger<AuthService> _logger;
    private readonly IPasswordHasher _hasher;

    public AuthService(IUserRepository users, IClock clock, ILogger<AuthService> logger, IPasswordHasher hasher)
    {
        this._users = users;
        this._clock = clock;
        this._logger = logger;
        this._hasher = hasher;
    }
    
    public async Task<User> RegisterAsync(string email, string password)
    {
        Email address = new Email(email);
        
        if (password.Length > MaxPasswordLength || password.Length < MinPasswordLength)
        {
            throw new DomainValidationException($"Haslo musi miec od {MinPasswordLength} do {MaxPasswordLength} znakow.");
        }

        if (await _users.GetByEmailAsync(address) is not null)
        {
            throw new DomainConflictException("Konto z tym adresem email istnieje.");
        }

        string passwordHashed = _hasher.Hash(password);
        User user = new User(address, passwordHashed, _clock.UtcNow);
        await _users.AddAsync(user);
        await _users.SaveChangesAsync();
        _logger.LogInformation("Zarejestrowano uzytkownika {UserId}", user.Id);
        return user;
    }

    public async Task<User?> ValidateCredentialsAsync(string email, string password)
    {
        User? user = null;
        if (Email.TryParse(email, out var address))
        {
            user = await _users.GetByEmailAsync(address!);
        }

        if (user is null)
        {
            _hasher.Hash(password);
            return null;
        }

        return _hasher.Verify(user.PasswordHash, password) == true ? user : null;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _users.GetByIdAsync(id); 
    }
}
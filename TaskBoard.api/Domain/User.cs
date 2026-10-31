namespace TaskBoard.Api.Domain;

public class User
{
    public Guid Id {get; private set;}
    public Email Email {get; private set;}
    public string PasswordHash {get; private set;}
    public DateTime CreatedAt {get; private set;}

    public User(Email email, string passwordHash, DateTime createdAt)
    {
        this.Id = Guid.CreateVersion7();
        this.Email = email;
        this.PasswordHash = RequireHash(passwordHash);
        this.CreatedAt = createdAt;
    }

    public void ChangePasswordHash(string passwordHash)
    {
        PasswordHash = RequireHash(passwordHash);
    }

    private static string RequireHash(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainValidationException("PasswordHash nie moze byc pusty.");
        }
        return value;
    }
}
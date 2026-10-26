namespace TaskBoard.Api.Domain;

public class Project
{
    public const int MaxNameLength = 100;

    public Guid OwnerId {get; private set;}
    public Guid Id {get; private set;}
    public string Name {get; private set;}
    public bool IsArchived {get; private set;}
    public DateTime CreatedAt {get; private set;}

    public Project(Guid ownerId, string Name, DateTime createdAt)
    {
        this.OwnerId = ownerId;
        this.Id = Guid.CreateVersion7();
        this.Name = NormalizeTitle(Name);
        this.CreatedAt = createdAt;
    }

    public bool IsOwnedBy(Guid ownerId) => this.OwnerId == ownerId;

    public void Rename(string newTitle)
    {
        EnsureNotArchived("Nie mozna zmienic tytulu zarchiwizowanemu projektowi.");
        this.Name = NormalizeTitle(newTitle);
    }

    public void Archive()
    {
        if (this.IsArchived)
        {
            throw new DomainConflictException("Nie mozna zarchiwizowac zarchiwizowanego projektu");
        }
        this.IsArchived = true;
    }

    public void Restore()
    {
        if (!this.IsArchived)
        {
            throw new DomainConflictException("Nie mozna zodzarchiwizowac zodzarchiwizowanego projektu");
        }
        this.IsArchived = true;
    }

    public TaskItem CreateTask(string title, DateTime createdAt)
    {
        EnsureNotArchived("Nie mozna dodac zadania do zarchiwizowanego projektu.");
        TaskItem task = new TaskItem(title, Id, createdAt);
        return task;
    }

    private void EnsureNotArchived(string message)
    {
        if (IsArchived)
        {
            throw new DomainConflictException(message);
        }
    }

    private static string NormalizeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainValidationException("Tytul projektu nie moze byc pusty.");
        }
        var trimmed = title.Trim();
        if (trimmed.Length > MaxNameLength)
        {
            throw new DomainValidationException($"Tytul projektu nie moze byc wiekszy niz: {MaxNameLength}");
        }
        return trimmed;
    }
}
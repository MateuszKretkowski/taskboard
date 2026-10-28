namespace TaskBoard.Api.Domain;

public class Project
{
    public const int MaxNameLength = 100;

    public Guid Id {get; private set;}
    public List<ProjectMember> Members {get; private set;}
    public string Name {get; private set;}
    public bool IsArchived {get; private set;}
    public DateTime CreatedAt {get; private set;}

    private Project()
    {
        this.Members = new List<ProjectMember>();
        this.Name = null!;
    }

    public Project(Guid ownerId, string Name, DateTime createdAt)
    {
        this.Id = Guid.CreateVersion7();
        this.Members = new List<ProjectMember>();
        this.Name = NormalizeTitle(Name);
        this.CreatedAt = createdAt;
        this.Members.Add(new ProjectMember(this.Id, ownerId, true));
    }

    public void AddMember(Guid userId, Guid newMemberId)
    {
        if (GetUser(newMemberId) is not null)
        {
            throw new DomainConflictException($"{newMemberId} nie moze byc czlonkiem projektu {Id} drugi raz.");
        }
        if (GetUser(userId) is null)
        {
            throw new InvalidOperationException($"{userId} nie jest czlonkem projektu {Id}.");
        }
        if (!GetUser(userId)!.IsOwner)
        {
            throw new DomainConflictException($"{userId} nie jest ownerem projektu {Id}");
        }
        Members.Add(new ProjectMember(Id, newMemberId, false));
    }

    public void RemoveMember(Guid userId, Guid newMemberId)
    {
        if (GetUser(newMemberId) is null)
        {
            throw new DomainConflictException($"{newMemberId} nie musi byc czlonkiem projektu {Id} drugi raz.");
        }
        if (GetUser(userId) is null)
        {
            throw new InvalidOperationException($"{userId} nie jest czlonkem projektu {Id}.");
        }
        if (!GetUser(userId)!.IsOwner)
        {
            throw new DomainConflictException($"{userId} nie jest ownerem projektu {Id}");
        }
        if (GetUser(newMemberId)!.IsOwner)
        {
            throw new DomainConflictException($"{userId} nie moze usunac sie z projektu {Id} bedac wlascicielem.");
        }
        Members.Remove(GetUser(newMemberId)!);
    }

    public ProjectMember? GetUser(Guid id)
    {
        foreach (ProjectMember member in Members)
        {
            if (member.UserId == id)
                return member;
        }
        return null;
    }

    public bool IsOwnedBy(Guid userId) => GetUser(userId) is not null ? GetUser(userId)!.IsOwner : false;

    public void Rename(string newTitle)
    {
        EnsureNotArchived("Nie mozna zmienic tytulu zarchiwizowanemu projektowi.");
        this.Name = NormalizeTitle(newTitle);
    }

    public void Archive(Guid userId)
    {
        if (this.IsArchived)
        {
            throw new DomainConflictException("Nie mozna zarchiwizowac zarchiwizowanego projektu");
        }
        if (!IsOwnedBy(userId))
        {
            throw new DomainConflictException("Nie mozna zarchiwizowac projektu nie bedac wlascicielem");
        }
        this.IsArchived = true;
    }

    public void Restore(Guid userId)
    {
        if (!this.IsArchived)
        {
            throw new DomainConflictException("Nie mozna zodzarchiwizowac zodzarchiwizowanego projektu");
        }
        if (!IsOwnedBy(userId))
        {
            throw new DomainConflictException("Nie mozna zarchiwizowac projektu nie bedac wlascicielem");
        }
        this.IsArchived = false;
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
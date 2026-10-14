namespace TaskBoard.Api.Domain;

public class Project
{
    public static int MaxNameLength;

    public Guid Id {get; private set;}
    public string Name {get; private set;}
    public bool IsArchived {get; private set;}

    public Project(string title)
    {
        this.Id = Guid.CreateVersion7();
        this.Name = NormalizeTitle(title);
        this.IsArchived = false;
    }

    public void Rename(string newTitle)
    {
        if (IsArchived)
        {
            throw new InvalidOperationException("Nie mozna zmienic tytulu zarchiwizowanemu projektowi.");
        }
        this.Name = NormalizeTitle(newTitle);
    }

    public void Archive()
    {
        if (this.IsArchived)
        {
            throw new InvalidOperationException("Nie mozna zarchiwizowac zarchiwizowanego projektu");
        }
        this.IsArchived = true;
    }

    public void Restore()
    {
        if (!this.IsArchived)
        {
            throw new InvalidOperationException("Nie mozna zodzarchiwizowac zodzarchiwizowanego projektu");
        }
        this.IsArchived = true;
    }

    public static string NormalizeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Tytul projektu nie moze byc pusty.", nameof(title));
        }
        var trimmed = title.Trim();
        if (trimmed.Length > MaxNameLength)
        {
            throw new ArgumentException($"Tytul projektu nie moze byc wiekszy niz: {MaxNameLength}");
        }
        return trimmed;
    }
}
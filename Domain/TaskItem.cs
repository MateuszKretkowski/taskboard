namespace TaskBoard.Api.Domain;

public class TaskItem 
{
    public const int MaxTitleLength = 200;

    public Guid Id { get; private set; }
    public string Title { get; private set; } = "";
    public TaskItemStatus Status {get; private set;}
    public DateTime CreatedAt {get; private set;}
    public DateTime? CompletedAt {get; private set;}

    public TaskItem(string title, DateTime dateTime)
    {
        this.Id = Guid.CreateVersion7();
        this.Title = NormalizeTitle(title);
        this.Status = TaskItemStatus.Todo;
        this.CreatedAt = dateTime;
    }

    public void Rename(string newTitle)
    {
        this.Title = NormalizeTitle(newTitle);
    }

    public void Start()
    {
        if (Status != TaskItemStatus.Todo)
        {
            throw new InvalidOperationException($"Nie mozna rozpoczac zadania w stanie: {Status}.");
        }
        this.Status = TaskItemStatus.InProgress;
    }

    public void Complete(DateTime completedAt)
    {
        if (Status != TaskItemStatus.InProgress)
        {
            throw new InvalidOperationException($"Nie mozna zakonczyc zadania w stanie: {Status}");
        }
        if (completedAt < CreatedAt)
        {
            throw new ArgumentException("Nie mozna zakonczyc zadania przed jego utworzeniem");
        }
        this.Status = TaskItemStatus.Done;
        this.CompletedAt = completedAt;
    }

    public void Reopen()
    {
        if (Status != TaskItemStatus.Done)
        {
            throw new InvalidOperationException($"Nie mozna ponownie otworzyc taska w stanie: {Status}");
        }
        this.Status = TaskItemStatus.InProgress;
        this.CompletedAt = null;
    }
    
    public static string NormalizeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Tytul tasku nie moze byc pusty.", nameof(title));
        }
        string trimmed = title.Trim();
        if (trimmed.Length > MaxTitleLength)
        {
            throw new ArgumentException($"Tytul tasku jest za dlugi. Max znakow: {MaxTitleLength}", nameof(title));
        }
        return trimmed;
    }
}
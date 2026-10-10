namespace TaskBoard.Api.Domain;

public class TaskItem 
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public TaskItemStatus Status {get; set;}
    public DateTime CreatedAt {get; set;}
    public DateTime? CompletedAt {get; set;}
}
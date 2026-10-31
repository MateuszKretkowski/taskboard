using TaskBoard.Api.Domain;

namespace TaskBoard.Api.Contracts;

public record TaskResponse(
    Guid Id,
    Guid ProjectId,
    string Title,
    TaskItemStatus Status,
    DateTime CreatedAt,
    DateTime? CompletedAt
)
{
    public static TaskResponse From(TaskItem task)
    {
        return new TaskResponse(task.Id, task.ProjectId, task.Title, task.Status, task.CreatedAt, task.CompletedAt);
    }
}
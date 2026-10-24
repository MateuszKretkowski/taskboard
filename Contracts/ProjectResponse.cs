using TaskBoard.Api.Domain;

namespace TaskBoard.Api.Contracts;

public record ProjectResponse(Guid Id, string Name, bool IsArchived, DateTime CreatedAt)
{
    public static ProjectResponse From(Project project)
    {
        return new ProjectResponse(project.Id, project.Name, project.IsArchived, project.CreatedAt);
    }
}
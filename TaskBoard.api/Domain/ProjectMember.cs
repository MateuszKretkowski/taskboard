namespace TaskBoard.Api.Domain;

public class ProjectMember
{
    public Guid Id {get; private set;}
    public Guid ProjectId {get; private set;}
    public Guid UserId {get; private set;}
    public bool IsOwner {get; private set;}

    public ProjectMember(Guid projectId, Guid userId, bool isOwner)
    {
        this.Id = Guid.CreateVersion7();
        this.ProjectId = projectId;
        this.UserId = userId;
        this.IsOwner = isOwner;
    }
}
namespace TaskBoard.Api.Contracts;

public record ManipulateMemberRequest(Guid userId, Guid projectId);
namespace TaskBoard.Api.Services;

public record ProjectSummary(Guid id, string Name, bool IsArchived, int OpenTaskCount, int DoneTaskCount);
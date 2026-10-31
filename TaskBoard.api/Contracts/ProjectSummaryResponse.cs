using TaskBoard.Api.Services;

namespace TaskBoard.Api.Contracts;

public record ProjectSummaryResponse(Guid Id, string Name, bool IsArchived, int OpenTaskCount, int DoneTaskCount)
{
    public static ProjectSummaryResponse From(ProjectSummary ps)
    {
        return new ProjectSummaryResponse(ps.id, ps.Name, ps.IsArchived, ps.OpenTaskCount, ps.DoneTaskCount);
    }
}
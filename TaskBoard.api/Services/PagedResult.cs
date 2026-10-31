namespace TaskBoard.Api.Services;

public record PagedResult<T>(IReadOnlyList<T> items, int page, int pageSize, int totalCount)
{
    public int totalPages => (int)Math.Ceiling(totalCount / (double)pageSize);
}
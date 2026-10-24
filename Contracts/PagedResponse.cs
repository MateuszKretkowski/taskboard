namespace TaskBoard.Api.Contracts;

using TaskBoard.Api.Services;

public record PagedResponse<T> (IReadOnlyList<T> Items, int Page, int PageSize, int TotalCount, int TotalPages)
{
    public static PagedResponse<T> From<TSource>(PagedResult<TSource> result, Func<TSource, T> map)
    {
        return new PagedResponse<T>
        (
            result.items.Select(map).ToList(),
            result.page,
            result.pageSize,
            result.totalCount,
            result.totalPages
        );
    }
}
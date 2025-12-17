namespace ZSports.Contracts.Common;

public record PaginatedResponse<T>
{
    public IEnumerable<T> Items { get; init; } = [];
    public int TotalItems { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}

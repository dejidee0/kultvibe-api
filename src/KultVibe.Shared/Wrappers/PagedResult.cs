namespace KultVibe.Shared.Wrappers;

/// <summary>
/// Shape for any paginated list payload, e.g. GET /api/admin/waitlist -&gt; { items[], total, page }.
/// </summary>
public sealed class PagedResult<T>
{
    public List<T> Items { get; init; } = new();
    public int Total { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling(Total / (double)PageSize);
}

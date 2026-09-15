using FCG.Catalog.Domain.Enums;

namespace FCG.Catalog.Domain.Shared.Filters;

public class ReviewFilter
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public Guid GameId { get; set; }
    public ReviewOrderBy OrderBy { get; set; } = ReviewOrderBy.CreatedAt;
    public bool Desc { get; set; } = false;
}

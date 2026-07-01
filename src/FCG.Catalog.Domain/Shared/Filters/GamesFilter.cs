using FCG.Catalog.Domain.Enums;

namespace FCG.Catalog.Domain.Shared.Filters;

public class GamesFilter
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public GameOrderBy OrderBy { get; set; } = GameOrderBy.Name;
    public bool Desc { get; set; } = false;
    public string? Search { get; set; } = null;
}

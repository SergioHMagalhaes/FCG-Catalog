using FCG.Catalog.Communication.Enums;

namespace FCG.Catalog.Communication.Requests;

public class RequestGetAllGamesJson
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public GameOrderBy OrderBy { get; set; } = GameOrderBy.Name;
    public bool Desc { get; set; } = false;
    public string? Search { get; set; } = null;
}

using FCG.Catalog.Communication.Enums;

namespace FCG.Catalog.Communication.Requests;

public class RequestGetAllGamesJson : RequestPagedBase
{
    public GameOrderBy OrderBy { get; set; } = GameOrderBy.Name;
    public string? Search { get; set; } = null;
}

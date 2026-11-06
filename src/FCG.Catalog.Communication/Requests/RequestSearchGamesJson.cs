namespace FCG.Catalog.Communication.Requests;

public class RequestSearchGamesJson
{
    public string Query { get; set; } = string.Empty;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}

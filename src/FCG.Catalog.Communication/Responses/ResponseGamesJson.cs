namespace FCG.Catalog.Communication.Responses;

public class ResponseGamesJson : ResponsePagedBase
{
    public List<ResponseShortGameJson> Games { get; set; } = [];
}

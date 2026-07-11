namespace FCG.Catalog.Communication.Responses;

public class ResponseUserLibraryJson
{
    public long Id { get; set; }
    public Guid ExternalId { get; set; }
    public Guid UserId { get; set; }
    public List<ResponseShortGameJson> Games { get; set; } = [];

}

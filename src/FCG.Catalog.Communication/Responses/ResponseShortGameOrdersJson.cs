using FCG.Catalog.Communication.Enums;

namespace FCG.Catalog.Communication.Responses;

public class ResponseShortGameOrdersJson
{
    public long Id { get; set; }
    public Guid ExternalId { get; set; }
    public long GameId { get; set; }
    public GameOrderStatus Status { get; set; }

}

using FCG.Catalog.Communication.Enums;

namespace FCG.Catalog.Communication.Responses;

public class ResponseGameOrderJson
{
    public Guid OrderId { get; set; }
    public long GameId { get; set; }
    public decimal Price { get; set; }
    public GameOrderStatus Status { get; set; }
}

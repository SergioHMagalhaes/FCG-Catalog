using FCG.Catalog.Communication.Enums;

namespace FCG.Catalog.Communication.Responses;

public class ResponseGameOrderJson
{
    public long Id { get; set; }
    public Guid ExternalId { get; set; }
    public long GameId { get; set; }
    public decimal Price { get; set; }
    public GameOrderStatus Status { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ProcessedOn { get; set; }
    public ResponseShortGameJson Game { get; set; } = default!;
}

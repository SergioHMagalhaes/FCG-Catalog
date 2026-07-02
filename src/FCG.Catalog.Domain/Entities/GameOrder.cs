using FCG.Catalog.Domain.Enums;

namespace FCG.Catalog.Domain.Entities;

public class GameOrder
{
    public long Id { get; set; }
    public Guid OrderId { get; set; }
    public long GameId { get; set; }
    public Game Game { get; set; } = default!;
    public Guid UserId { get; set; }
    public decimal Price { get; set; }
    public GameOrderStatus Status { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ProcessedOn { get; set; }

    public void Approve()
    {
        Status = GameOrderStatus.Approved;
        ProcessedOn = DateTime.UtcNow;
    }

    public void Reject()
    {
        Status = GameOrderStatus.Rejected;
        ProcessedOn = DateTime.UtcNow;
    }
}

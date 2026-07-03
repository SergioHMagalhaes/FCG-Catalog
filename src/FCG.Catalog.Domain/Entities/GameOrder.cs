using FCG.Catalog.Domain.Enums;

namespace FCG.Catalog.Domain.Entities;

public class GameOrder
{
    public long Id { get; private set; }
    public Guid ExternalId { get; private set; }
    public long GameId { get; private set; }
    public Game Game { get; private set; } = default!;
    public Guid UserId { get; private set; }
    public decimal Price { get; private set; }
    public GameOrderStatus Status { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime? ProcessedOn { get; private set; }

    private GameOrder() {}
    public GameOrder(Game game, Guid userId)
    {
        ExternalId = Guid.NewGuid();
        GameId = game.Id;
        Price = game.Price;
        UserId = userId;
        Status = GameOrderStatus.Pending;
        CreatedOn = DateTime.UtcNow;
    }
}

using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Entities;
using FCG.Shared.Events;

namespace FCG.Catalog.Application.Extensions;

public static class GameOrderExtensions
{
    public static ResponseGameOrderJson MapToResponse(this GameOrder gameOrder)
    {
        return new ResponseGameOrderJson
        {
            OrderId = gameOrder.OrderId,
            GameId = gameOrder.GameId,
            Price = gameOrder.Price,
            Status = (Communication.Enums.GameOrderStatus)gameOrder.Status
        };
    }

    public static OrderPlacedEvent MapToEvent(this GameOrder gameOrder, Game game)
    {
        return new OrderPlacedEvent
        (
            OrderId: gameOrder.OrderId,
            GameId: game.ExternalId,
            GameName: game.Name,
            UserId: gameOrder.UserId,
            Amount: gameOrder.Price,
            CreatedOn: gameOrder.CreatedOn
        );
    }
}

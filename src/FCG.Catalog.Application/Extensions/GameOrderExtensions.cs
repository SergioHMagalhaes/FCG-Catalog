using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Entities;
using FCG.Shared.Events;

namespace FCG.Catalog.Application.Extensions;

public static class GameOrderExtensions
{
    public static ResponsePlaceGameOrderJson MapToResponse(this GameOrder gameOrder)
    {
        return new ResponsePlaceGameOrderJson
        {
            ExternalId = gameOrder.ExternalId,
            GameId = gameOrder.GameId,
            Price = gameOrder.Price,
            Status = (Communication.Enums.GameOrderStatus)gameOrder.Status
        };
    }

    public static OrderPlacedEvent MapToEvent(this GameOrder gameOrder, Game game)
    {
        return new OrderPlacedEvent
        (
            OrderId: gameOrder.ExternalId,
            GameId: game.ExternalId,
            GameName: game.Name,
            UserId: gameOrder.UserId,
            Amount: gameOrder.Price,
            CreatedOn: gameOrder.CreatedOn
        );
    }

    public static ResponseGameOrdersJson MapToResponse(this IEnumerable<GameOrder> gameOrders)
    {
        return new ResponseGameOrdersJson
        {
            GameOrders = gameOrders.Select(o => new ResponseShortGameOrdersJson
            {
                Id = o.Id,
                ExternalId = o.ExternalId,
                GameId = o.GameId,
                Status = (Communication.Enums.GameOrderStatus)o.Status
            }).ToList()
        };
    }

    public static ResponseGameOrderJson MapToGameOrderResponse(this GameOrder gameOrder)
    {
        return new ResponseGameOrderJson
        {
            Id = gameOrder.Id,
            ExternalId = gameOrder.ExternalId,
            GameId = gameOrder.GameId,
            Status = (Communication.Enums.GameOrderStatus)gameOrder.Status,
            Price = gameOrder.Price,
            CreatedOn = gameOrder.CreatedOn,
            ProcessedOn = gameOrder.ProcessedOn,
            Game = new ResponseShortGameJson
            {
                Id = gameOrder.Game.Id,
                ExternalId = gameOrder.Game.ExternalId,
                Name = gameOrder.Game.Name,
                Price = gameOrder.Game.Price
            }
        };
    }
}

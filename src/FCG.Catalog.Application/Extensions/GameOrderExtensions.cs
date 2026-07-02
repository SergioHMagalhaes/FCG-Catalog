using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Entities;

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
}

using FCG.Catalog.Communication.Responses;

namespace FCG.Catalog.Application.UseCases.GameOrders.GetUserOrders;

public interface IGetUserGamerOrderUseCase
{
    Task<ResponseGameOrdersJson> Execute();
}

using FCG.Catalog.Communication.Responses;

namespace FCG.Catalog.Application.UseCases.GameOrders.GetById;

public interface IGetGameOrderByIdUseCase
{
    Task<ResponseGameOrderJson> Execute(long id);
}

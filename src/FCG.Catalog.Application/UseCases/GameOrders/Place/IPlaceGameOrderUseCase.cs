using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;

namespace FCG.Catalog.Application.UseCases.GameOrders.Place;

public interface IPlaceGameOrderUseCase
{
    Task<ResponsePlaceGameOrderJson> Execute(RequestPlaceGameOrderJson request);
}

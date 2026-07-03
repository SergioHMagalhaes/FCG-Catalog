using FCG.Catalog.Application.Extensions;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Messaging;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Services.LoggedUser;

namespace FCG.Catalog.Application.UseCases.GameOrders.GetUserOrders;

public class GetUserGamerOrderUseCase : IGetUserGamerOrderUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IGameOrderRepository _repository;
    public GetUserGamerOrderUseCase(
        ILoggedUser loggedUser,
        IGameOrderRepository repository)
    {
        _loggedUser = loggedUser;
        _repository = repository;
    }
    public async Task<ResponseGameOrdersJson> Execute()
    {
        var userId = _loggedUser.GetId();

        var orders = await _repository.GetByUserId(userId);

        return orders.MapToResponse();
    }
}

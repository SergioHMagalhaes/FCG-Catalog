using FCG.Catalog.Application.Extensions;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Services.LoggedUser;
using FCG.Catalog.Exception.ExceptionsBase;

namespace FCG.Catalog.Application.UseCases.GameOrders.GetById;

public class GetGameOrderByIdUseCase : IGetGameOrderByIdUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IGameOrderRepository _repository;
    public GetGameOrderByIdUseCase(
        ILoggedUser loggedUser,
        IGameOrderRepository repository)
    {
        _loggedUser = loggedUser;
        _repository = repository;
    }

    public async Task<ResponseGameOrderJson> Execute(long id)
    {
        var userId = _loggedUser.GetId();

        var result = await _repository.GetById(id, userId);

        if (result is null)
            throw new NotFoundException("Ordem não encontrada");

        return result.MapToGameOrderResponse();
    }
}

using FCG.Catalog.Application.Extensions;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Services.LoggedUser;
using FCG.Catalog.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace FCG.Catalog.Application.UseCases.GameOrders.Place;

public class PlaceGameOrderUseCase : IPlaceGameOrderUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IGameOrderRepository _repository;
    private readonly IGameRepository _gameRepository;
    private readonly IUnitOfWork _unitOfWork;
    public PlaceGameOrderUseCase(
        ILoggedUser loggedUser,
        IGameOrderRepository repository,
        IGameRepository gameRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _gameRepository = gameRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResponseGameOrderJson> Execute(RequestPlaceGameOrderJson request)
    {
        var game = await _gameRepository.GetByExternalId(request.GameId);
        await Validate(request, game);

        var userId = _loggedUser.GetId();

        var gameOrder = new GameOrder(game!, userId);

        await _repository.Add(gameOrder);
        await _unitOfWork.Commit();

        return gameOrder.MapToResponse();
    }

    private async Task Validate(RequestPlaceGameOrderJson request, Game? gameExists)
    {
        var result = new PlaceGameOrderValidator().Validate(request);

        if (gameExists is null)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, "Jogo não existe."));
        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}

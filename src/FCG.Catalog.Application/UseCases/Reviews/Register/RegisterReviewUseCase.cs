using FCG.Catalog.Application.Extensions;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Services.LoggedUser;
using FCG.Catalog.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace FCG.Catalog.Application.UseCases.Reviews.Register;

public class RegisterReviewUseCase : IRegisterReviewUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IGameRepository _gameRepository;
    private readonly IReviewRepository _repository;
    public RegisterReviewUseCase(
        ILoggedUser loggedUser,
        IGameRepository gameRepository,
        IReviewRepository repository)
    {
        _loggedUser = loggedUser;
        _gameRepository = gameRepository;
        _repository = repository;
    }

    public async Task<ResponseRegisterdReviewJson> Execute(RequestReviewJson request)
    {
        var game = await _gameRepository.GetByExternalId(request.GameId);
        var userId = _loggedUser.GetId();
        var userName = _loggedUser.GetName();

        await Validate(request, game);

        var review = new Review(
            request.GameId,
            userId,
            userName,
            request.Rating,
            request.Comment,
            request.Tags
        );

        await _repository.Add(review);

        return review.MapToResponse();
    }

    private async Task Validate(RequestReviewJson request, Game? gameExists)
    {
        var result = new RegisterReviewValidator().Validate(request);

        if (gameExists == null)
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

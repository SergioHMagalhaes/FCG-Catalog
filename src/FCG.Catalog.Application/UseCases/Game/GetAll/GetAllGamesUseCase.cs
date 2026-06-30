using FCG.Catalog.Application.Extensions;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Exception.ExceptionsBase;

namespace FCG.Catalog.Application.UseCases.Game.GetAll;

public class GetAllGamesUseCase : IGetAllGamesUseCase
{
    private readonly IGameRepository _repository;

    public GetAllGamesUseCase(IGameRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseGamesJson> Execute(RequestGetAllGamesJson request)
    {
        Validate(request);

        var filter = request.MapToDomain();
        var result = await _repository.GetAll(filter);

        return result.MapToResponse();
    }

    private void Validate(RequestGetAllGamesJson request)
    {
        var validator = new GetAllGamesValidator();
        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessage = result.Errors.Select(f => f.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessage);
        }
    }
}

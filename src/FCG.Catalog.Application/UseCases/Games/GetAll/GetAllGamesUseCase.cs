using FCG.Catalog.Application.Extensions;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Constants;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Services.Caching;
using FCG.Catalog.Exception.ExceptionsBase;

namespace FCG.Catalog.Application.UseCases.Games.GetAll;

public class GetAllGamesUseCase : IGetAllGamesUseCase
{
    private readonly IGameRepository _repository;
    private readonly ICacheService _cacheService;

    public GetAllGamesUseCase(IGameRepository repository, ICacheService cacheService)
    {
        _repository = repository;
        _cacheService = cacheService;
    }

    public async Task<ResponseGamesJson> Execute(RequestGetAllGamesJson request)
    {
        Validate(request);

        var cacheKey = CacheKeys.Games.List(
            request.Page,
            request.PageSize,
            (int)request.OrderBy,
            request.Desc,
            request.Search);

        var cachedResponse = await _cacheService.GetAsync<ResponseGamesJson>(cacheKey);
        if (cachedResponse is not null)
        {
            return cachedResponse;
        }

        var filter = request.MapToDomain();
        var result = await _repository.GetAll(filter);
        var response = result.MapToResponse();

        await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(10));

        return response;
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

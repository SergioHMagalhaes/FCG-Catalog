using FCG.Catalog.Application.Extensions;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Constants;
using FCG.Catalog.Domain.Services.Caching;
using FCG.Catalog.Domain.Services.Search;
using FCG.Catalog.Exception.ExceptionsBase;

namespace FCG.Catalog.Application.UseCases.Games.Search;

public class SearchGamesUseCase : ISearchGamesUseCase
{
    private readonly IGameSearchService _searchService;
    private readonly ICacheService _cacheService;

    public SearchGamesUseCase(IGameSearchService searchService, ICacheService cacheService)
    {
        _searchService = searchService;
        _cacheService = cacheService;
    }

    public async Task<ResponseGamesJson> Execute(RequestSearchGamesJson request)
    {
        Validate(request);

        var cacheKey = CacheKeys.Games.Search(request.Query, request.Page, request.PageSize);

        var cachedResponse = await _cacheService.GetAsync<ResponseGamesJson>(cacheKey);
        if (cachedResponse is not null)
        {
            return cachedResponse;
        }

        var result = await _searchService.SearchAsync(request.Query, request.Page, request.PageSize);
        var response = result.MapToResponse();

        await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(10));

        return response;
    }

    private static void Validate(RequestSearchGamesJson request)
    {
        var result = new SearchGamesValidator().Validate(request);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}

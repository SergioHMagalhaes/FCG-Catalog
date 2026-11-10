using FCG.Catalog.Application.Extensions;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Services.Search;
using FCG.Catalog.Exception.ExceptionsBase;

namespace FCG.Catalog.Application.UseCases.Games.Search;

public class SearchGamesUseCase : ISearchGamesUseCase
{
    private readonly IGameSearchService _searchService;

    public SearchGamesUseCase(IGameSearchService searchService)
    {
        _searchService = searchService;
    }

    public async Task<ResponseGamesJson> Execute(RequestSearchGamesJson request)
    {
        Validate(request);

        var result = await _searchService.SearchAsync(request.Query, request.Page, request.PageSize);

        return result.MapToResponse();
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

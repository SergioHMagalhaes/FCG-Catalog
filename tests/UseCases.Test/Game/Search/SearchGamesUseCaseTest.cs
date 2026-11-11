using CommonTestUtilities.Entities;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services;
using FCG.Catalog.Application.UseCases.Games.Search;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Domain.Shared.Pagination;
using FCG.Catalog.Exception.ExceptionsBase;

namespace UseCases.Test.Game.Search;

public class SearchGamesUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestSearchGamesJsonBuilder.Build();
        var games = GameBuilder.Collection(3);
        var pagedResult = new PagedResult<FCG.Catalog.Domain.Entities.Game>(
            games,
            TotalCount: games.Count,
            Page: request.Page,
            PageSize: request.PageSize);

        var useCase = CreateUseCase(pagedResult);

        var result = await useCase.Execute(request);

        Assert.NotNull(result);
        Assert.Equal(games.Count, result.Games.Count);
        Assert.Equal(pagedResult.TotalCount, result.TotalCount);
    }

    [Fact]
    public async Task Error_Query_Empty()
    {
        var request = RequestSearchGamesJsonBuilder.Build();
        request.Query = string.Empty;

        var useCase = CreateUseCase(new PagedResult<FCG.Catalog.Domain.Entities.Game>([], 0, request.Page, request.PageSize));

        async Task<FCG.Catalog.Communication.Responses.ResponseGamesJson> act() => await useCase.Execute(request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Error_Page_Invalid(int page)
    {
        var request = RequestSearchGamesJsonBuilder.Build();
        request.Page = page;

        var useCase = CreateUseCase(new PagedResult<FCG.Catalog.Domain.Entities.Game>([], 0, request.Page, request.PageSize));

        async Task<FCG.Catalog.Communication.Responses.ResponseGamesJson> act() => await useCase.Execute(request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
    }

    private static ISearchGamesUseCase CreateUseCase(PagedResult<FCG.Catalog.Domain.Entities.Game> result)
    {
        var searchServiceBuilder = new GameSearchServiceBuilder().Search(result);

        return new SearchGamesUseCase(searchServiceBuilder.Build(), CacheServiceBuilder.Build());
    }
}

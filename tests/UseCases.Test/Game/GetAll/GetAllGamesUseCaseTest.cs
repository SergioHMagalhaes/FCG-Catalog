using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FCG.Catalog.Application.UseCases.Game.GetAll;
using FCG.Catalog.Domain.Shared.Filters;
using FCG.Catalog.Domain.Shared.Pagination;

namespace UseCases.Test.Game.GetAll;

internal class CreateGetAllGameUseCase
{
    public required GetAllGamesUseCase UseCase;
    public required GameRepositoryBuilder Repository;
    public required GamesFilter Filter;
}

public class GetAllGamesUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestGetAllGamesJsonBuilder.Build();
        var games = GameBuilder.Collection(5);
        var pagedResult = new PagedResult<FCG.Catalog.Domain.Entities.Game>(
            games,
            TotalCount: 20,
            Page: request.Page,
            PageSize: request.PageSize);
        var useCase = CreateUseCase(pagedResult, request).UseCase;

        var result = await useCase.Execute(request);

        Assert.NotNull(result);
        Assert.Equal(games.Count, result.Games.Count);
        Assert.Equal(pagedResult.TotalCount, result.TotalCount);
        Assert.Equal(pagedResult.Page, result.Page);
        Assert.Equal(pagedResult.PageSize, result.PageSize);
        Assert.Equal(pagedResult.TotalPages, result.TotalPages);
        Assert.Equal(pagedResult.HasNextPage, result.HasNextPage);
        Assert.Equal(pagedResult.HasPreviousPage, result.HasPreviousPage);
    }

    [Fact]
    public async Task Success_Should_Return_Games()
    {
        var request = RequestGetAllGamesJsonBuilder.Build();
        var games = GameBuilder.Collection(3);
        var pagedResult = new PagedResult<FCG.Catalog.Domain.Entities.Game>(
            games,
            TotalCount: games.Count,
            Page: request.Page,
            PageSize: request.PageSize);
        var useCase = CreateUseCase(pagedResult, request).UseCase;

        var result = await useCase.Execute(request);

        Assert.Collection(result.Games,
            game =>
            {
                Assert.Equal(games[0].Id, game.Id);
                Assert.Equal(games[0].Name, game.Name);
                Assert.Equal(games[0].Price, game.Price);
            },
            game =>
            {
                Assert.Equal(games[1].Id, game.Id);
                Assert.Equal(games[1].Name, game.Name);
                Assert.Equal(games[1].Price, game.Price);
            },
            game =>
            {
                Assert.Equal(games[2].Id, game.Id);
                Assert.Equal(games[2].Name, game.Name);
                Assert.Equal(games[2].Price, game.Price);
            });
    }

    [Fact]
    public async Task Success_Should_Get_All_Games_By_Filter()
    {
        var request = RequestGetAllGamesJsonBuilder.Build();
        var games = GameBuilder.Collection(3);
        var pagedResult = new PagedResult<FCG.Catalog.Domain.Entities.Game>(
            games,
            TotalCount: 12,
            Page: request.Page,
            PageSize: request.PageSize);
        var sut = CreateUseCase(pagedResult, request);
        var useCase = sut.UseCase;
        var repository = sut.Repository;

        await useCase.Execute(request);

        repository.VerifyGetAll(sut.Filter);
    }

    private CreateGetAllGameUseCase CreateUseCase(PagedResult<FCG.Catalog.Domain.Entities.Game> games, FCG.Catalog.Communication.Requests.RequestGetAllGamesJson request)
    {
        var filter = new GamesFilter
        {
            Page = request.Page,
            PageSize = request.PageSize,
            OrderBy = (FCG.Catalog.Domain.Enums.GameOrderBy)request.OrderBy,
            Desc = request.Desc,
            Search = request.Search
        };
        var repository = new GameRepositoryBuilder();
        repository.GetAll(games, filter);

        return new CreateGetAllGameUseCase
        {
            UseCase = new GetAllGamesUseCase(repository.Build()),
            Repository = repository,
            Filter = filter
        };
    }
}

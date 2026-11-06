using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services;
using FCG.Catalog.Application.UseCases.Games.Delete;
using FCG.Catalog.Exception.ExceptionsBase;

namespace UseCases.Test.Game.Delete;

public class DeleteGameUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var game = GameBuilder.Build();
        var sut = CreateUseCase(game);

        await sut.UseCase.Execute(game.Id);
    }

    [Fact]
    public async Task Error_Category_Not_Found()
    {
        var sut = CreateUseCase();

        async Task act() => await sut.UseCase.Execute(id: 1000);

        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    [Fact]
    public async Task Success_Should_Delete_From_Index()
    {
        var game = GameBuilder.Build();
        var sut = CreateUseCase(game);

        await sut.UseCase.Execute(game.Id);

        sut.SearchService.VerifyDeleteOnce(game.Id);
    }

    [Fact]
    public async Task Error_Category_Not_Found_Should_Not_Delete_From_Index()
    {
        var sut = CreateUseCase();

        async Task act() => await sut.UseCase.Execute(id: 1000);

        await Assert.ThrowsAsync<NotFoundException>(act);

        sut.SearchService.VerifyDeleteNever();
    }

    private (DeleteGameUseCase UseCase, GameSearchServiceBuilder SearchService) CreateUseCase(FCG.Catalog.Domain.Entities.Game? game = null)
    {
        var repository = new GameRepositoryBuilder();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var cacheService = CacheServiceBuilder.Build();
        var searchServiceBuilder = new GameSearchServiceBuilder();

        if (game is not null)
            repository.GetByIdTracked(game);
        else
            repository.GetByIdTrackedNotFound();

        var useCase = new DeleteGameUseCase(repository.Build(), unitOfWork, cacheService, searchServiceBuilder.Build());

        return (useCase, searchServiceBuilder);
    }
}

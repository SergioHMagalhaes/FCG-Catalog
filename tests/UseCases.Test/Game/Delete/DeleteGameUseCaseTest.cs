using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using FCG.Catalog.Application.UseCases.Category.Delete;
using FCG.Catalog.Application.UseCases.Game.Delete;
using FCG.Catalog.Exception.ExceptionsBase;

namespace UseCases.Test.Game.Delete;

public class DeleteGameUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var game = GameBuilder.Build();
        var useCase = CreateUseCase(game);

        await useCase.Execute(game.Id);
    }

    [Fact]
    public async Task Error_Category_Not_Found()
    {
        var useCase = CreateUseCase();

        async Task act() => await useCase.Execute(id: 1000);

        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    private DeleteGameUseCase CreateUseCase(FCG.Catalog.Domain.Entities.Game? game = null)
    {
        var repository = new GameRepositoryBuilder();
        var unitOfWork = new UnitOfWorkBuilder().Build();

        if (game is not null)
            repository.GetByIdTracked(game);
        else
            repository.GetByIdTrackedNotFound();

        return new DeleteGameUseCase(repository.Build(), unitOfWork);
    }
}

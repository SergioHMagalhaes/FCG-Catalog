using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using FCG.Catalog.Application.UseCases.Game.GetById;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Exception.ExceptionsBase;

namespace UseCases.Test.Game.GetById;

public class GetGameByIdUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var game = GameBuilder.Build();
        var useCase = CreateUseCase(game);

        var result = await useCase.Execute(game.Id);

        Assert.NotNull(result);
        Assert.Equal(game.Id, result.Id);
        Assert.Equal(game.Name, result.Name);
        Assert.Equal(game.Description, result.Description);
        Assert.Equal(game.Price, result.Price);
        Assert.NotNull(result.Category);
        Assert.Equal(game.Category.Id, result.Category.Id);
        Assert.Equal(game.Category.Name, result.Category.Name);
    }

    [Fact]
    public async Task Error_Game_Not_Found()
    {
        var game = GameBuilder.Build();
        var useCase = CreateUseCase(game);

        async Task<ResponseGameJson> act() => await useCase.Execute(1000);

        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    private GetGameByIdUseCase CreateUseCase(FCG.Catalog.Domain.Entities.Game game)
    {
        var repository = new GameRepositoryBuilder().GetById(game).Build();

        return new GetGameByIdUseCase(repository);
    }
}

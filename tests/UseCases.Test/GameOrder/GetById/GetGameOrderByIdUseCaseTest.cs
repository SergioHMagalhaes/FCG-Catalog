using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services;
using FCG.Catalog.Application.UseCases.GameOrders.GetById;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Exception.ExceptionsBase;

namespace UseCases.Test.GameOrder.GetById;

internal class Sut
{
    public required GetGameOrderByIdUseCase UseCase;
    public required GameOrderRepositoryBuilder Repository;
    public required Guid UserId;
}

public class GetGameOrderByIdUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var userId = Guid.NewGuid();
        var game = GameBuilder.Build();
        var gameOrder = GameOrderBuilder.Build(game, userId);
        var useCase = CreateSut(userId, gameOrder).UseCase;

        var result = await useCase.Execute(gameOrder.Id);

        Assert.NotNull(result);
        Assert.Equal(gameOrder.Id, result.Id);
        Assert.Equal(gameOrder.ExternalId, result.ExternalId);
        Assert.Equal(gameOrder.GameId, result.GameId);
        Assert.Equal((FCG.Catalog.Communication.Enums.GameOrderStatus)gameOrder.Status, result.Status);
        Assert.Equal(game.Id, result.Game.Id);
        Assert.Equal(game.ExternalId, result.Game.ExternalId);
        Assert.Equal(game.Name, result.Game.Name);
        Assert.Equal(game.Price, result.Game.Price);
    }

    [Fact]
    public async Task Success_Should_Get_Game_Order_By_Id_And_Logged_User()
    {
        var userId = Guid.NewGuid();
        var game = GameBuilder.Build();
        var gameOrder = GameOrderBuilder.Build(game, userId);
        var sut = CreateSut(userId, gameOrder);

        await sut.UseCase.Execute(gameOrder.Id);

        sut.Repository.VerifyGetById(gameOrder.Id, sut.UserId);
    }

    [Fact]
    public async Task Error_Game_Order_Not_Found()
    {
        var userId = Guid.NewGuid();
        var gameOrder = GameOrderBuilder.Collection(userId, 1).First();
        var useCase = CreateSut(userId, null).UseCase;

        async Task<ResponseGameOrderJson> act() => await useCase.Execute(gameOrder.Id);

        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    private Sut CreateSut(Guid userId, FCG.Catalog.Domain.Entities.GameOrder? gameOrder)
    {
        var repository = new GameOrderRepositoryBuilder();

        if (gameOrder is not null)
            repository.GetById(gameOrder.Id, userId, gameOrder);

        var loggedUser = LoggedUserBuilder.Build(userId);

        return new Sut
        {
            UseCase = new GetGameOrderByIdUseCase(loggedUser, repository.Build()),
            Repository = repository,
            UserId = userId
        };
    }
}

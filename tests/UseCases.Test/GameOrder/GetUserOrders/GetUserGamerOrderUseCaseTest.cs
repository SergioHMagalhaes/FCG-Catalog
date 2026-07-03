using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services;
using FCG.Catalog.Application.UseCases.GameOrders.GetUserOrders;

namespace UseCases.Test.GameOrder.GetUserOrders;

internal class Sut
{
    public required GetUserGamerOrderUseCase UseCase;
    public required GameOrderRepositoryBuilder Repository;
    public required Guid UserId;
}

public class GetUserGamerOrderUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var userId = Guid.NewGuid();
        var gameOrders = GameOrderBuilder.Collection(userId, 5);
        var useCase = CreateSut(userId, gameOrders).UseCase;

        var result = await useCase.Execute();

        Assert.NotNull(result);
        Assert.Equal(gameOrders.Count, result.GameOrders.Count);
    }

    [Fact]
    public async Task Success_Should_Return_Game_Orders()
    {
        var userId = Guid.NewGuid();
        var gameOrders = GameOrderBuilder.Collection(userId, 3);
        var useCase = CreateSut(userId, gameOrders).UseCase;

        var result = await useCase.Execute();

        Assert.Collection(result.GameOrders,
            gameOrder =>
            {
                Assert.Equal(gameOrders[0].Id, gameOrder.Id);
                Assert.Equal(gameOrders[0].GameId, gameOrder.GameId);
                Assert.Equal((FCG.Catalog.Communication.Enums.GameOrderStatus)gameOrders[0].Status, gameOrder.Status);
            },
            gameOrder =>
            {
                Assert.Equal(gameOrders[1].Id, gameOrder.Id);
                Assert.Equal(gameOrders[1].GameId, gameOrder.GameId);
                Assert.Equal((FCG.Catalog.Communication.Enums.GameOrderStatus)gameOrders[1].Status, gameOrder.Status);
            },
            gameOrder =>
            {
                Assert.Equal(gameOrders[2].Id, gameOrder.Id);
                Assert.Equal(gameOrders[2].GameId, gameOrder.GameId);
                Assert.Equal((FCG.Catalog.Communication.Enums.GameOrderStatus)gameOrders[2].Status, gameOrder.Status);
            });
    }

    [Fact]
    public async Task Success_Should_Get_Game_Orders_By_Logged_User()
    {
        var userId = Guid.NewGuid();
        var gameOrders = GameOrderBuilder.Collection(userId, 3);
        var sut = CreateSut(userId, gameOrders);

        await sut.UseCase.Execute();

        sut.Repository.VerifyGetByUserId(sut.UserId);
    }

    private Sut CreateSut(Guid userId, List<FCG.Catalog.Domain.Entities.GameOrder> gameOrders)
    {
        var repository = new GameOrderRepositoryBuilder();
        repository.GetByUserId(userId, gameOrders);
        var loggedUser = LoggedUserBuilder.Build(userId);

        return new Sut
        {
            UseCase = new GetUserGamerOrderUseCase(loggedUser, repository.Build()),
            Repository = repository,
            UserId = userId
        };
    }
}

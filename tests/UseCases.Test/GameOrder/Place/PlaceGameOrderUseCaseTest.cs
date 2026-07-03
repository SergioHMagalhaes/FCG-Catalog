using CommonTestUtilities.Entities;
using CommonTestUtilities.Messaging;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services;
using FCG.Catalog.Application.UseCases.GameOrders.Place;
using FCG.Catalog.Communication.Enums;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Exception.ExceptionsBase;

namespace UseCases.Test.GameOrder.Place;

internal class Sut
{
    public required PlaceGameOrderUseCase UseCase;
    public required UnitOfWorkBuilder UnitOfWork;
    public required EventPublisherBuilder EventPublisher;
}

public class PlaceGameOrderUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestPlaceGameOrderJsonBuilder.Build();
        var game = GameBuilder.Build();
        var sut = CreateSut(request, game);

        var response = await sut.UseCase.Execute(request);

        Assert.NotNull(response);
        Assert.Equal(game.Id, response.GameId);
        Assert.Equal(game.Price, response.Price);
        Assert.Equal(GameOrderStatus.Pending, response.Status);
    }

    [Fact]
    public async Task Success_Should_Call_Commit()
    {
        var request = RequestPlaceGameOrderJsonBuilder.Build();
        var game = GameBuilder.Build();
        var sut = CreateSut(request, game);

        await sut.UseCase.Execute(request);

        sut.UnitOfWork.VerifyCommitOnce();
    }

    [Fact]
    public async Task Error_Game_Not_Found_Should_Throw_NotFoundException()
    {
        var request = RequestPlaceGameOrderJsonBuilder.Build();
        var sut = CreateSut();

        async Task<ResponsePlaceGameOrderJson> act() => await sut.UseCase.Execute(request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        sut.UnitOfWork.VerifyCommitNever();
        sut.EventPublisher.VerifyPublishNever();
    }

    [Fact]
    public async Task Error_Game_Not_Found_Should_Not_Call_Commit()
    {
        var request = RequestPlaceGameOrderJsonBuilder.Build();
        var sut = CreateSut();

        async Task<ResponsePlaceGameOrderJson> act() => await sut.UseCase.Execute(request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        sut.UnitOfWork.VerifyCommitNever();
        sut.EventPublisher.VerifyPublishNever();
    }

    [Fact]
    public async Task Error_Empty_GameId_Should_Throw_ValidationException()
    {
        var request = RequestPlaceGameOrderJsonBuilder.Build();
        request.GameId = Guid.Empty;

        var sut = CreateSut();

        var act = async () => await sut.UseCase.Execute(request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        sut.UnitOfWork.VerifyCommitNever();
        sut.EventPublisher.VerifyPublishNever();
    }

    [Fact]
    public async Task Success_Should_Publish_Game_Order_Placed_Event()
    {
        var request = RequestPlaceGameOrderJsonBuilder.Build();
        var game = GameBuilder.Build();
        var sut = CreateSut(request, game);

        await sut.UseCase.Execute(request);

        sut.EventPublisher.VerifyPublishOrderPlacedEventOnce();
    }

    private Sut CreateSut(RequestPlaceGameOrderJson? request = null, FCG.Catalog.Domain.Entities.Game? game = null)
    {
        var unitOfWorkBuilder = new UnitOfWorkBuilder();
        var unitOfWork = unitOfWorkBuilder.Build();
        var repository = new GameOrderRepositoryBuilder().Build();
        var gameRepository = new GameRepositoryBuilder();
        var loggedUser = LoggedUserBuilder.Build(request?.GameId ?? Guid.NewGuid());
        var eventPublisher = new EventPublisherBuilder();


        if (request != null)
        {
            var gameEntity = game ?? GameBuilder.Build();
            gameEntity.ExternalId = request.GameId;

            gameRepository.GetByExternalId(gameEntity);
        }

        return new Sut
        {
            UseCase = new PlaceGameOrderUseCase(loggedUser, eventPublisher.Build(), repository, gameRepository.Build(), unitOfWork),
            UnitOfWork = unitOfWorkBuilder,
            EventPublisher = eventPublisher
        };
    }
}

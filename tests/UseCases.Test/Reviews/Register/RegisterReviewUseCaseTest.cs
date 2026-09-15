using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services;
using FCG.Catalog.Application.UseCases.Reviews.Register;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Exception.ExceptionsBase;

namespace UseCases.Test.Reviews.Register;

internal class Sut
{
    public required RegisterReviewUseCase UseCase;
    public required ReviewRepositoryBuilder Repository;
    public required Guid UserId;
}

public class RegisterReviewUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestReviewJsonBuilder.Build();
        var sut = CreateSut(request);

        var response = await sut.UseCase.Execute(request);

        Assert.NotNull(response);
        Assert.NotEqual(Guid.Empty, response.Id);
    }

    [Fact]
    public async Task Success_Should_Call_Add()
    {
        var request = RequestReviewJsonBuilder.Build();
        var sut = CreateSut(request);

        await sut.UseCase.Execute(request);

        sut.Repository.VerifyAddOnce();
    }

    [Fact]
    public async Task Error_Game_Not_Found_Should_Throw_ValidationException()
    {
        var request = RequestReviewJsonBuilder.Build();
        var sut = CreateSut();

        async Task<ResponseRegisterdReviewJson> act() => await sut.UseCase.Execute(request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        sut.Repository.VerifyAddNever();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(6)]
    public async Task Error_Rating_Invalid_Should_Throw_ValidationException(int rating)
    {
        var request = RequestReviewJsonBuilder.Build();
        request.Rating = rating;
        var sut = CreateSut(request);

        async Task<ResponseRegisterdReviewJson> act() => await sut.UseCase.Execute(request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        sut.Repository.VerifyAddNever();
    }

    [Fact]
    public async Task Error_Comment_Empty_Should_Throw_ValidationException()
    {
        var request = RequestReviewJsonBuilder.Build();
        request.Comment = string.Empty;
        var sut = CreateSut(request);

        async Task<ResponseRegisterdReviewJson> act() => await sut.UseCase.Execute(request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        sut.Repository.VerifyAddNever();
    }

    [Fact]
    public async Task Error_Comment_TooLong_Should_Throw_ValidationException()
    {
        var request = RequestReviewJsonBuilder.Build();
        request.Comment = new string('a', 501);
        var sut = CreateSut(request);

        async Task<ResponseRegisterdReviewJson> act() => await sut.UseCase.Execute(request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        sut.Repository.VerifyAddNever();
    }

    [Fact]
    public async Task Error_Tags_MoreThanFive_Should_Throw_ValidationException()
    {
        var request = RequestReviewJsonBuilder.Build();
        request.Tags = ["tag1", "tag2", "tag3", "tag4", "tag5", "tag6"];
        var sut = CreateSut(request);

        async Task<ResponseRegisterdReviewJson> act() => await sut.UseCase.Execute(request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        sut.Repository.VerifyAddNever();
    }

    [Fact]
    public async Task Error_Tags_TooLong_Should_Throw_ValidationException()
    {
        var request = RequestReviewJsonBuilder.Build();
        request.Tags = [new string('a', 31)];
        var sut = CreateSut(request);

        async Task<ResponseRegisterdReviewJson> act() => await sut.UseCase.Execute(request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        sut.Repository.VerifyAddNever();
    }

    [Fact]
    public async Task Error_Tags_Duplicated_Should_Throw_ValidationException()
    {
        var request = RequestReviewJsonBuilder.Build();
        request.Tags = ["tag", "TAG"];
        var sut = CreateSut(request);

        async Task<ResponseRegisterdReviewJson> act() => await sut.UseCase.Execute(request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        sut.Repository.VerifyAddNever();
    }

    private Sut CreateSut(RequestReviewJson? request = null, FCG.Catalog.Domain.Entities.Game? game = null)
    {
        var repository = new ReviewRepositoryBuilder();
        var gameRepository = new GameRepositoryBuilder();
        var userId = Guid.NewGuid();
        var loggedUser = LoggedUserBuilder.Build(userId);

        if (request != null)
        {
            var gameEntity = game ?? GameBuilder.Build();
            gameEntity.ExternalId = request.GameId;

            gameRepository.GetByExternalId(gameEntity);
        }

        return new Sut
        {
            UseCase = new RegisterReviewUseCase(loggedUser, gameRepository.Build(), repository.Build()),
            Repository = repository,
            UserId = userId
        };
    }
}

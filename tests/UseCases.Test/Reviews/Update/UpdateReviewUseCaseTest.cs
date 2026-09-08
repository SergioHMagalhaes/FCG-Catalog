using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services;
using FCG.Catalog.Application.UseCases.Reviews.Update;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Exception.ExceptionsBase;

namespace UseCases.Test.Reviews.Update;

internal class Sut
{
    public required UpdateReviewUseCase UseCase;
    public required ReviewRepositoryBuilder Repository;
    public required Review Review;
}

public class UpdateReviewUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestReviewUpdateJsonBuilder.Build();
        var sut = CreateSut(request);

        await sut.UseCase.Execute(sut.Review.Id, request);

        Assert.Equal(request.Rating, sut.Review.Rating);
        Assert.Equal(request.Comment, sut.Review.Comment);
    }

    [Fact]
    public async Task Success_Should_Call_Update()
    {
        var request = RequestReviewUpdateJsonBuilder.Build();
        var sut = CreateSut(request);

        await sut.UseCase.Execute(sut.Review.Id, request);

        sut.Repository.VerifyUpdateOnce();
    }

    [Fact]
    public async Task Error_Review_Not_Found_Should_Throw_NotFoundException()
    {
        var request = RequestReviewUpdateJsonBuilder.Build();
        var sut = CreateSut(request, found: false);

        async Task act() => await sut.UseCase.Execute(sut.Review.Id, request);

        await Assert.ThrowsAsync<NotFoundException>(act);
        sut.Repository.VerifyUpdateNever();
    }

    [Fact]
    public async Task Error_User_Not_Owner_Should_Throw_UnauthorizedAccessException()
    {
        var request = RequestReviewUpdateJsonBuilder.Build();
        var sut = CreateSut(request, ownerMatchesLoggedUser: false);

        async Task act() => await sut.UseCase.Execute(sut.Review.Id, request);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
        sut.Repository.VerifyUpdateNever();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(6)]
    public async Task Error_Rating_Invalid_Should_Throw_ValidationException(int rating)
    {
        var request = RequestReviewUpdateJsonBuilder.Build();
        request.Rating = rating;
        var sut = CreateSut(request);

        async Task act() => await sut.UseCase.Execute(sut.Review.Id, request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        sut.Repository.VerifyUpdateNever();
    }

    [Fact]
    public async Task Error_Comment_Empty_Should_Throw_ValidationException()
    {
        var request = RequestReviewUpdateJsonBuilder.Build();
        request.Comment = string.Empty;
        var sut = CreateSut(request);

        async Task act() => await sut.UseCase.Execute(sut.Review.Id, request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        sut.Repository.VerifyUpdateNever();
    }

    [Fact]
    public async Task Error_Comment_TooLong_Should_Throw_ValidationException()
    {
        var request = RequestReviewUpdateJsonBuilder.Build();
        request.Comment = new string('a', 501);
        var sut = CreateSut(request);

        async Task act() => await sut.UseCase.Execute(sut.Review.Id, request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        sut.Repository.VerifyUpdateNever();
    }

    [Fact]
    public async Task Error_Tags_MoreThanFive_Should_Throw_ValidationException()
    {
        var request = RequestReviewUpdateJsonBuilder.Build();
        request.Tags = ["tag1", "tag2", "tag3", "tag4", "tag5", "tag6"];
        var sut = CreateSut(request);

        async Task act() => await sut.UseCase.Execute(sut.Review.Id, request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        sut.Repository.VerifyUpdateNever();
    }

    [Fact]
    public async Task Error_Tags_TooLong_Should_Throw_ValidationException()
    {
        var request = RequestReviewUpdateJsonBuilder.Build();
        request.Tags = [new string('a', 31)];
        var sut = CreateSut(request);

        async Task act() => await sut.UseCase.Execute(sut.Review.Id, request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        sut.Repository.VerifyUpdateNever();
    }

    [Fact]
    public async Task Error_Tags_Duplicated_Should_Throw_ValidationException()
    {
        var request = RequestReviewUpdateJsonBuilder.Build();
        request.Tags = ["tag", "TAG"];
        var sut = CreateSut(request);

        async Task act() => await sut.UseCase.Execute(sut.Review.Id, request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        sut.Repository.VerifyUpdateNever();
    }

    private Sut CreateSut(RequestReviewUpdateJson request, bool found = true, bool ownerMatchesLoggedUser = true)
    {
        var review = ReviewBuilder.Build();
        var repository = new ReviewRepositoryBuilder();

        if (found)
            repository.GetById(review);
        else
            repository.GetByIdNotFound();

        var loggedUserId = ownerMatchesLoggedUser ? review.UserId : Guid.NewGuid();
        var loggedUser = LoggedUserBuilder.Build(loggedUserId);

        return new Sut
        {
            UseCase = new UpdateReviewUseCase(loggedUser, repository.Build()),
            Repository = repository,
            Review = review
        };
    }
}

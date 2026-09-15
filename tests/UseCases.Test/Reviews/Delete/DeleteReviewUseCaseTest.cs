using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services;
using FCG.Catalog.Application.UseCases.Reviews.Delete;
using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Exception.ExceptionsBase;

namespace UseCases.Test.Reviews.Delete;

public class DeleteReviewUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var review = ReviewBuilder.Build();
        var repository = new ReviewRepositoryBuilder();
        var useCase = CreateUseCase(review, repository);

        await useCase.Execute(review.Id);

        repository.VerifyDeleteOnce(review.Id);
    }

    [Fact]
    public async Task Success_When_User_Is_Admin_And_Not_Owner()
    {
        var review = ReviewBuilder.Build();
        var repository = new ReviewRepositoryBuilder();
        var useCase = CreateUseCase(review, repository, ownerMatchesLoggedUser: false, isAdmin: true);

        await useCase.Execute(review.Id);

        repository.VerifyDeleteOnce(review.Id);
    }

    [Fact]
    public async Task Error_Review_Not_Found_Should_Throw_NotFoundException()
    {
        var repository = new ReviewRepositoryBuilder();
        var useCase = CreateUseCase(review: null, repository);

        async Task act() => await useCase.Execute(Guid.NewGuid());

        await Assert.ThrowsAsync<NotFoundException>(act);
        repository.VerifyDeleteNever();
    }

    [Fact]
    public async Task Error_User_Not_Owner_Should_Throw_UnauthorizedException()
    {
        var review = ReviewBuilder.Build();
        var repository = new ReviewRepositoryBuilder();
        var useCase = CreateUseCase(review, repository, ownerMatchesLoggedUser: false);

        async Task act() => await useCase.Execute(review.Id);

        await Assert.ThrowsAsync<UnauthorizedException>(act);
        repository.VerifyDeleteNever();
    }

    private DeleteReviewUseCase CreateUseCase(
        Review? review,
        ReviewRepositoryBuilder repository,
        bool ownerMatchesLoggedUser = true,
        bool isAdmin = false)
    {
        if (review is not null)
            repository.GetById(review);
        else
            repository.GetByIdNotFound();

        var loggedUserId = ownerMatchesLoggedUser ? review?.UserId ?? Guid.NewGuid() : Guid.NewGuid();
        var loggedUser = LoggedUserBuilder.Build(loggedUserId, isAdmin: isAdmin);

        return new DeleteReviewUseCase(loggedUser, repository.Build());
    }
}

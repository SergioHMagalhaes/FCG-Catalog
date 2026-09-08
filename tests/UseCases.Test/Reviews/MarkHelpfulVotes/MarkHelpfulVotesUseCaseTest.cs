using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using FCG.Catalog.Application.UseCases.Reviews.MarkHelpfulVotes;
using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Exception.ExceptionsBase;

namespace UseCases.Test.Reviews.MarkHelpfulVotes;

public class MarkHelpfulVotesUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var review = ReviewBuilder.Build();
        var repository = new ReviewRepositoryBuilder();
        var useCase = CreateUseCase(review, repository);

        await useCase.Execute(review.Id);

        Assert.Equal(1, review.HelpfulVotes);
    }

    [Fact]
    public async Task Success_Should_Call_Update()
    {
        var review = ReviewBuilder.Build();
        var repository = new ReviewRepositoryBuilder();
        var useCase = CreateUseCase(review, repository);

        await useCase.Execute(review.Id);

        repository.VerifyUpdateOnce();
    }

    [Fact]
    public async Task Error_Review_Not_Found_Should_Throw_NotFoundException()
    {
        var repository = new ReviewRepositoryBuilder();
        var useCase = CreateUseCase(review: null, repository);

        async Task act() => await useCase.Execute(Guid.NewGuid());

        await Assert.ThrowsAsync<NotFoundException>(act);
        repository.VerifyUpdateNever();
    }

    private MarkHelpfulVotesUseCase CreateUseCase(Review? review, ReviewRepositoryBuilder repository)
    {
        if (review is not null)
            repository.GetById(review);
        else
            repository.GetByIdNotFound();

        return new MarkHelpfulVotesUseCase(repository.Build());
    }
}

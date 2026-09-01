using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FCG.Catalog.Application.UseCases.Reviews.GetByGameId;
using FCG.Catalog.Domain.Shared.Filters;
using FCG.Catalog.Domain.Shared.Pagination;

namespace UseCases.Test.Reviews.GetByGameId;

internal class CreateGetReviewsByGameIdUseCase
{
    public required GetReviewsByGameIdUseCase UseCase;
    public required ReviewRepositoryBuilder Repository;
    public required ReviewFilter Filter;
}

public class GetReviewsByGameIdUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestGetReviewByGameIdJsonBuilder.Build();
        var reviews = ReviewBuilder.Collection(5);
        var pagedResult = new PagedResult<FCG.Catalog.Domain.Entities.Review>(
            reviews,
            TotalCount: 20,
            Page: request.Page,
            PageSize: request.PageSize);
        var useCase = CreateUseCase(pagedResult, request).UseCase;

        var result = await useCase.Execute(request);

        Assert.NotNull(result);
        Assert.Equal(reviews.Count, result.Reviews.Count);
        Assert.Equal(pagedResult.TotalCount, result.TotalCount);
        Assert.Equal(pagedResult.Page, result.Page);
        Assert.Equal(pagedResult.PageSize, result.PageSize);
        Assert.Equal(pagedResult.TotalPages, result.TotalPages);
        Assert.Equal(pagedResult.HasNextPage, result.HasNextPage);
        Assert.Equal(pagedResult.HasPreviousPage, result.HasPreviousPage);
    }

    [Fact]
    public async Task Success_Should_Return_Reviews()
    {
        var request = RequestGetReviewByGameIdJsonBuilder.Build();
        var reviews = ReviewBuilder.Collection(3);
        var pagedResult = new PagedResult<FCG.Catalog.Domain.Entities.Review>(
            reviews,
            TotalCount: reviews.Count,
            Page: request.Page,
            PageSize: request.PageSize);
        var useCase = CreateUseCase(pagedResult, request).UseCase;

        var result = await useCase.Execute(request);

        Assert.Collection(result.Reviews,
            review =>
            {
                Assert.Equal(reviews[0].Id, review.Id);
                Assert.Equal(reviews[0].GameId, review.GameId);
                Assert.Equal(reviews[0].Rating, review.Rating);
            },
            review =>
            {
                Assert.Equal(reviews[1].Id, review.Id);
                Assert.Equal(reviews[1].GameId, review.GameId);
                Assert.Equal(reviews[1].Rating, review.Rating);
            },
            review =>
            {
                Assert.Equal(reviews[2].Id, review.Id);
                Assert.Equal(reviews[2].GameId, review.GameId);
                Assert.Equal(reviews[2].Rating, review.Rating);
            });
    }

    [Fact]
    public async Task Success_Should_Get_Reviews_By_Filter()
    {
        var request = RequestGetReviewByGameIdJsonBuilder.Build();
        var reviews = ReviewBuilder.Collection(3);
        var pagedResult = new PagedResult<FCG.Catalog.Domain.Entities.Review>(
            reviews,
            TotalCount: 12,
            Page: request.Page,
            PageSize: request.PageSize);
        var sut = CreateUseCase(pagedResult, request);
        var useCase = sut.UseCase;
        var repository = sut.Repository;

        await useCase.Execute(request);

        repository.VerifyGetByGameId(sut.Filter);
    }

    private CreateGetReviewsByGameIdUseCase CreateUseCase(PagedResult<FCG.Catalog.Domain.Entities.Review> reviews, FCG.Catalog.Communication.Requests.RequestGetReviewByGameIdJson request)
    {
        var filter = new ReviewFilter
        {
            Page = request.Page,
            PageSize = request.PageSize,
            GameId = request.GameId,
            OrderBy = (FCG.Catalog.Domain.Enums.ReviewOrderBy)request.OrderBy,
            Desc = request.Desc
        };
        var repository = new ReviewRepositoryBuilder();
        repository.GetByGameId(reviews, filter);

        return new CreateGetReviewsByGameIdUseCase
        {
            UseCase = new GetReviewsByGameIdUseCase(repository.Build()),
            Repository = repository,
            Filter = filter
        };
    }
}

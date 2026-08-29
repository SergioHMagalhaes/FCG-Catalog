using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Shared.Filters;
using FCG.Catalog.Domain.Shared.Pagination;
using Moq;

namespace CommonTestUtilities.Repositories;

public class ReviewRepositoryBuilder
{
    private readonly Mock<IReviewRepository> _repository;

    public ReviewRepositoryBuilder()
    {
        _repository = new Mock<IReviewRepository>();
    }

    public void VerifyAddOnce()
    {
        _repository.Verify(repository => repository.Add(It.IsAny<Review>()), Times.Once);
    }

    public void VerifyAddNever()
    {
        _repository.Verify(repository => repository.Add(It.IsAny<Review>()), Times.Never);
    }

    public ReviewRepositoryBuilder GetByGameId(PagedResult<Review> reviews, ReviewFilter filter)
    {
        _repository.Setup(repository => repository.GetByGameId(
            It.Is<ReviewFilter>(actualFilter =>
                actualFilter.Page == filter.Page
                && actualFilter.PageSize == filter.PageSize
                && actualFilter.GameId == filter.GameId
                && actualFilter.OrderBy == filter.OrderBy
                && actualFilter.Desc == filter.Desc))).ReturnsAsync(reviews);

        return this;
    }

    public void VerifyGetByGameId(ReviewFilter filter)
    {
        _repository.Verify(repository => repository.GetByGameId(
            It.Is<ReviewFilter>(actualFilter =>
                actualFilter.Page == filter.Page
                && actualFilter.PageSize == filter.PageSize
                && actualFilter.GameId == filter.GameId
                && actualFilter.OrderBy == filter.OrderBy
                && actualFilter.Desc == filter.Desc)), Times.Once);
    }

    public IReviewRepository Build() => _repository.Object;
}

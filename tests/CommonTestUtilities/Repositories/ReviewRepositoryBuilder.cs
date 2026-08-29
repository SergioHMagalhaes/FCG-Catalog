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

    public ReviewRepositoryBuilder GetById(Review review)
    {
        _repository.Setup(repository => repository.GetById(review.Id)).ReturnsAsync(review);

        return this;
    }

    public ReviewRepositoryBuilder GetByIdNotFound()
    {
        _repository.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync((Review?)null);

        return this;
    }

    public void VerifyUpdateOnce()
    {
        _repository.Verify(repository => repository.Update(It.IsAny<Review>()), Times.Once);
    }

    public void VerifyUpdateNever()
    {
        _repository.Verify(repository => repository.Update(It.IsAny<Review>()), Times.Never);
    }

    public IReviewRepository Build() => _repository.Object;
}

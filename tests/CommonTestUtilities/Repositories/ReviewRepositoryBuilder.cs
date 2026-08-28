using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Repositories;
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

    public IReviewRepository Build() => _repository.Object;
}

using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories;

public class LibraryRepositoryBuilder
{
    private readonly Mock<ILibraryRepository> _repository;

    public LibraryRepositoryBuilder()
    {
        _repository = new Mock<ILibraryRepository>();
    }

    public LibraryRepositoryBuilder GetByUserId(Guid userId, Library? library)
    {
        _repository
            .Setup(repository => repository.GetByUserId(userId))
            .ReturnsAsync(library);

        return this;
    }

    public void VerifyGetByUserId(Guid userId)
    {
        _repository.Verify(repository => repository.GetByUserId(userId), Times.Once);
    }

    public ILibraryRepository Build() => _repository.Object;
}

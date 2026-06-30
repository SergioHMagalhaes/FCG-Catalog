using FCG.Catalog.Domain.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories;

public class GameRepositoryBuild
{
    private readonly Mock<IGameRepository> _repository;

    public GameRepositoryBuild()
    {
        _repository = new Mock<IGameRepository>();
    }

    public IGameRepository Build() => _repository.Object;
}

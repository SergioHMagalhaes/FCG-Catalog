using FCG.Catalog.Domain.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories;

public class GameOrderRepositoryBuilder
{
    private readonly Mock<IGameOrderRepository> _repository;

    public GameOrderRepositoryBuilder()
    {
        _repository = new Mock<IGameOrderRepository>();
    }

    public IGameOrderRepository Build() => _repository.Object;
}

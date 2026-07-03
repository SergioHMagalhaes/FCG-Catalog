using FCG.Catalog.Domain.Entities;
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

    public GameOrderRepositoryBuilder GetByUserId(Guid userId, List<GameOrder> gameOrders)
    {
        _repository
            .Setup(repository => repository.GetByUserId(userId))
            .ReturnsAsync(gameOrders);

        return this;
    }

    public GameOrderRepositoryBuilder GetById(long id, Guid userId, GameOrder? gameOrder)
    {
        _repository
            .Setup(repository => repository.GetById(id, userId))
            .ReturnsAsync(gameOrder);

        return this;
    }

    public void VerifyGetByUserId(Guid userId)
    {
        _repository.Verify(repository => repository.GetByUserId(userId), Times.Once);
    }

    public void VerifyGetById(long id, Guid userId)
    {
        _repository.Verify(repository => repository.GetById(id, userId), Times.Once);
    }

    public IGameOrderRepository Build() => _repository.Object;
}

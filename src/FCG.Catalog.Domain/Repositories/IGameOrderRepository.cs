using FCG.Catalog.Domain.Entities;

namespace FCG.Catalog.Domain.Repositories;

public interface IGameOrderRepository
{
    Task Add(GameOrder gameOrder);
    Task<List<GameOrder>> GetByUserId(Guid userId);
}

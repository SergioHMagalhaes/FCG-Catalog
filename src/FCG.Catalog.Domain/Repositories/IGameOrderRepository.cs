using FCG.Catalog.Domain.Entities;

namespace FCG.Catalog.Domain.Repositories;

public interface IGameOrderRepository
{
    Task Add(GameOrder gameOrder);
}

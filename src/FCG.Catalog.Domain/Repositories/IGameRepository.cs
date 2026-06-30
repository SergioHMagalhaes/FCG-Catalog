using FCG.Catalog.Domain.Entities;

namespace FCG.Catalog.Domain.Repositories;

public interface IGameRepository
{
    Task Add(Game game);
}

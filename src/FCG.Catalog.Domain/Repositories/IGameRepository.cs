using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Shared.Filters;
using FCG.Catalog.Domain.Shared.Pagination;

namespace FCG.Catalog.Domain.Repositories;

public interface IGameRepository
{
    Task Add(Game game);
    Task<PagedResult<Game>> GetAll(GamesFilter filter);
    Task<Game?> GetById(long id);
    Task<Game?> GetByIdTracked(long id);
}

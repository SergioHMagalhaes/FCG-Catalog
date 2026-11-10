using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Shared.Pagination;

namespace FCG.Catalog.Domain.Services.Search;

public interface IGameSearchService
{
    Task EnsureIndexCreatedAsync(CancellationToken cancellationToken = default);
    Task IndexAsync(Game game, CancellationToken cancellationToken = default);
    Task DeleteAsync(long gameId, CancellationToken cancellationToken = default);
    Task<PagedResult<Game>> SearchAsync(string query, int page, int pageSize, CancellationToken cancellationToken = default);
}

using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Shared.Filters;
using FCG.Catalog.Domain.Shared.Pagination;

namespace FCG.Catalog.Domain.Repositories;

public interface IReviewRepository
{
    Task Add(Review review);

    Task<PagedResult<Review>> GetByGameId(ReviewFilter filter);
}

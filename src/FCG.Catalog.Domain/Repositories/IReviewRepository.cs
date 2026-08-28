using FCG.Catalog.Domain.Entities;

namespace FCG.Catalog.Domain.Repositories;

public interface IReviewRepository
{
    Task Add(Review review);
}

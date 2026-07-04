using FCG.Catalog.Domain.Entities;

namespace FCG.Catalog.Domain.Repositories;

public interface ILibraryRepository
{
    Task<Library?> GetByUserId(Guid userId);
}

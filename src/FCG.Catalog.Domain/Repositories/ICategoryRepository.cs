using FCG.Catalog.Domain.Entities;

namespace FCG.Catalog.Domain.Repositories;

public interface ICategoryRepository
{
    Task Add(Category category);
    Task<bool> ExistsByName(string name);
}

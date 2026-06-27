using FCG.Catalog.Domain.Entities;

namespace FCG.Catalog.Domain.Repositories;

public interface ICategoryRepository
{
    Task Add(Category category);
    Task<bool> ExistsByName(string name);
    Task<List<Category>> GetAll();
    Task<Category?> GetById(long id);
    Task<Category?> GetByIdTracked(long id);
    void Update(Category category);
    Task Delete(long id);
}

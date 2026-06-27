using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FCG.Catalog.Infrastructure.DataAccess.Repositories;

internal class CategoryRepository(ApplicationDbContext context) : ICategoryRepository
{
    private readonly ApplicationDbContext _dbContext = context;

    public async Task Add(Category category)
    {
        await _dbContext.Categories.AddAsync(category);
    }

    public async Task<bool> ExistsByName(string name)
    {
        var normalizedName = name.Trim().ToUpper();

        return await _dbContext.Categories
            .AnyAsync(category => category.Name.ToUpper() == normalizedName);
    }

    public Task<List<Category>> GetAll()
    {
        return _dbContext.Categories.AsNoTracking().ToListAsync();
    }

    public async Task<Category?> GetById(long id)
    {
        return await _dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(category => category.Id == id);
    }
}

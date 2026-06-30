using FCG.Catalog.Domain.Repositories;

namespace FCG.Catalog.Infrastructure.DataAccess;

internal class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = dbContext;

    public async Task Commit() => await _context.SaveChangesAsync();
}

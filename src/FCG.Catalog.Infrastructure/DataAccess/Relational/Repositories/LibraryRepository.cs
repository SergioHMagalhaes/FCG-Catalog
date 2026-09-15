using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FCG.Catalog.Infrastructure.DataAccess.Relational.Repositories;

internal class LibraryRepository(ApplicationDbContext context) : ILibraryRepository
{
    private readonly ApplicationDbContext _dbContext = context;

    public async Task<Library?> GetByUserId(Guid userId)
    {
        return await _dbContext.Libraries
            .AsNoTracking()
            .Include(library => library.Games)
            .FirstOrDefaultAsync(library => library.UserId == userId);
    }
}

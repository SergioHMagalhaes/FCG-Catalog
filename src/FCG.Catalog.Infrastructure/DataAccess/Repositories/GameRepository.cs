using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Repositories;

namespace FCG.Catalog.Infrastructure.DataAccess.Repositories;

internal class GameRepository(ApplicationDbContext context) : IGameRepository
{
    private readonly ApplicationDbContext _dbContext = context;

    public async Task Add(Game game)
    {
        await _dbContext.Games.AddAsync(game);
    }
}

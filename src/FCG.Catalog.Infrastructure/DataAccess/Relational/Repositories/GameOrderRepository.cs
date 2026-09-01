using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Enums;
using FCG.Catalog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FCG.Catalog.Infrastructure.DataAccess.Relational.Repositories;

internal class GameOrderRepository(ApplicationDbContext context) : IGameOrderRepository
{
    private readonly ApplicationDbContext _dbContext = context;
    
    public async Task Add(GameOrder gameOrder)
    {
        await _dbContext.GameOrders.AddAsync(gameOrder);
    }

    public async Task<bool> ExistsActiveOrder(long gameId, Guid userId)
    {
        return await _dbContext.GameOrders
            .AsNoTracking()
            .AnyAsync(gameOrder =>
                gameOrder.GameId == gameId &&
                gameOrder.UserId == userId &&
                (
                    gameOrder.Status == GameOrderStatus.Approved ||
                    gameOrder.Status == GameOrderStatus.Pending
                ));
    }

    public Task<GameOrder?> GetById(long id, Guid userId)
    {
        return _dbContext.GameOrders
            .AsNoTracking()
            .Include(gameOrder => gameOrder.Game)
            .FirstOrDefaultAsync(gameOrder => gameOrder.Id == id && gameOrder.UserId == userId);
    }

    public async Task<List<GameOrder>> GetByUserId(Guid userId)
    {
        return await _dbContext.GameOrders
            .AsNoTracking()
            .Where(gameOrder => gameOrder.UserId == userId)
            .ToListAsync();
    }
}

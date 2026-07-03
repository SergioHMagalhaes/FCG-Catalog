using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FCG.Catalog.Infrastructure.DataAccess.Repositories;

public class GameOrderRepository(ApplicationDbContext context) : IGameOrderRepository
{
    private readonly ApplicationDbContext _dbContext = context;
    
    public async Task Add(GameOrder gameOrder)
    {
        await _dbContext.GameOrders.AddAsync(gameOrder);
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

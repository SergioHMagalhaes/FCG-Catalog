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
}

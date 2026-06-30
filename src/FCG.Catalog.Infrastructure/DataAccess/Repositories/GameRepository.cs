using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Enums;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Shared.Filters;
using FCG.Catalog.Domain.Shared.Pagination;
using Microsoft.EntityFrameworkCore;

namespace FCG.Catalog.Infrastructure.DataAccess.Repositories;

internal class GameRepository(ApplicationDbContext context) : IGameRepository
{
    private readonly ApplicationDbContext _dbContext = context;

    public async Task Add(Game game)
    {
        await _dbContext.Games.AddAsync(game);
    }

    public async Task<PagedResult<Game>> GetAll(GamesFilter filter)
    {
        var query = _dbContext.Games.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = query.Where(x => x.Name.Contains(filter.Search));


        query = (filter.OrderBy, filter.Desc) switch
        {
            (GameOrderBy.Name, true) => query.OrderByDescending(x => x.Name),
            (GameOrderBy.Name, false) => query.OrderBy(x => x.Name),
            (GameOrderBy.Price, true) => query.OrderByDescending(x => x.Price),
            (GameOrderBy.Price, false) => query.OrderBy(x => x.Price),
            (GameOrderBy.Category, true) => query.OrderByDescending(x => x.CategoryId),
            _ => query.OrderBy(x => x.Name),
        };

        var total = await query.CountAsync();

        var items = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return new PagedResult<Game>(items, total, filter.Page, filter.PageSize);
    }
}

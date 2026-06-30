using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Shared.Filters;
using FCG.Catalog.Domain.Shared.Pagination;

namespace FCG.Catalog.Application.Extensions;

public static class GameExtensions
{
    public static Game MapToDomain(this RequestGameJson request)
    {
        return new Game
        {
            Name = request.Name,
            Description = request.Description,
            CategoryId = request.CategoryId,
            Price = request.Price
        };
    }

    public static GamesFilter MapToDomain(this RequestGetAllGamesJson request)
    {
        return new GamesFilter
        {
            Page = request.Page,
            PageSize = request.PageSize,
            OrderBy = (Domain.Enums.GameOrderBy)request.OrderBy,
            Desc = request.Desc,
            Search = request.Search
        };
    }

    public static ResponseGamesJson MapToResponse(this PagedResult<Game> pagedResult)
    {
        return new ResponseGamesJson
        {
            Games = pagedResult.Items.Select(game => new ResponseShortGameJson
            {
                Id = game.Id,
                Name = game.Name,
                Price = game.Price
            }).ToList(),
            Page = pagedResult.Page,
            PageSize = pagedResult.PageSize,
            TotalCount = pagedResult.TotalCount,
            TotalPages = pagedResult.TotalPages,
            HasNextPage = pagedResult.HasNextPage,
            HasPreviousPage = pagedResult.HasPreviousPage
        };
    }
}

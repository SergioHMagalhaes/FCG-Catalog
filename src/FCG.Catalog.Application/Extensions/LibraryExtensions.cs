using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Entities;

namespace FCG.Catalog.Application.Extensions;

public static class LibraryExtensions
{
    public static ResponseUserLibraryJson MapToResponse(this Library library)
    {
        return new ResponseUserLibraryJson
        {
            Id = library.Id,
            ExternalId = library.ExternalId,
            UserId = library.UserId,
            Games = [.. library.Games.Select(game => new ResponseShortGameJson
            {
                Id = game.Id,
                ExternalId = game.ExternalId,
                Name = game.Name,
                Price = game.Price
            })]
        };
    }
}

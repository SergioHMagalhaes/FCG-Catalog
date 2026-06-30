using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Domain.Entities;

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
}

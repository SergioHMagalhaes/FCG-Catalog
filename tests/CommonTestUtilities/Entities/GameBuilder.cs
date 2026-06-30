using Bogus;
using FCG.Catalog.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class GameBuilder
{
    public static List<Game> Collection(uint count = 2)
    {
        var list = new List<Game>();

        if (count == 0)
            count = 1;

        var gameId = 1;

        for (int i = 0; i < count; i++)
        {
            var game = Build();
            game.Id = gameId++;

            list.Add(game);
        }

        return list;
    }

    public static Game Build()
    {
        return new Faker<Game>()
            .RuleFor(g => g.Id, _ => 1)
            .RuleFor(g => g.Name, f => f.Commerce.ProductName())
            .RuleFor(g => g.Description, f => f.Commerce.ProductDescription())
            .RuleFor(g => g.Price, f => f.Random.Decimal(1, 500))
            .RuleFor(g => g.CategoryId, _ => 1);
    }
}

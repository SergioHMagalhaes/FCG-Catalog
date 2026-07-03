using Bogus;
using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Enums;

namespace CommonTestUtilities.Entities;

public class GameOrderBuilder
{
    public static List<GameOrder> Collection(Guid userId, uint count = 2)
    {
        var list = new List<GameOrder>();

        if (count == 0)
            count = 1;

        var gameId = 1;

        for (int i = 0; i < count; i++)
        {
            var game = GameBuilder.Build();
            game.Id = gameId++;

            list.Add(Build(game, userId));
        }

        return list;
    }

    public static GameOrder Build(Game game, Guid userId)
    {
        return new Faker<GameOrder>()
            .CustomInstantiator(_ => new GameOrder(game, userId))
            .RuleFor(gameOrder => gameOrder.Game, _ => game)
            .RuleFor(gameOrder => gameOrder.Id, f => f.Random.Long(1, 1000))
            .RuleFor(gameOrder => gameOrder.Status, f => f.PickRandom<GameOrderStatus>());
    }
}

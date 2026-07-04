using Bogus;
using FCG.Catalog.Domain.Entities;
using System.Reflection;

namespace CommonTestUtilities.Entities;

public class LibraryBuilder
{
    public static Library Build(Guid userId, uint gamesCount = 2)
    {
        var games = GameBuilder.Collection(gamesCount);

        return new Faker<Library>()
            .RuleFor(library => library.Id, f => f.Random.Long(1, 1000))
            .RuleFor(library => library.ExternalId, _ => Guid.NewGuid())
            .RuleFor(library => library.UserId, _ => userId)
            .FinishWith((_, library) => AddGames(library, games));
    }

    private static void AddGames(Library library, List<Game> games)
    {
        var field = typeof(Library).GetField("_games", BindingFlags.NonPublic | BindingFlags.Instance);

        var libraryGames = (List<Game>)field!.GetValue(library)!;

        libraryGames.AddRange(games);
    }
}

using Bogus;
using FCG.Catalog.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class ReviewBuilder
{
    public static List<Review> Collection(uint count = 2)
    {
        var list = new List<Review>();

        if (count == 0)
            count = 1;

        for (int i = 0; i < count; i++)
            list.Add(Build());

        return list;
    }

    public static Review Build(Guid? gameId = null)
    {
        return new Faker<Review>()
            .CustomInstantiator(f => new Review(
                gameId ?? Guid.NewGuid(),
                Guid.NewGuid(),
                f.Internet.UserName(),
                f.Random.Int(1, 5),
                f.Lorem.Sentence(),
                f.Make(2, () => f.Lorem.Word()).Distinct().ToList()));
    }
}

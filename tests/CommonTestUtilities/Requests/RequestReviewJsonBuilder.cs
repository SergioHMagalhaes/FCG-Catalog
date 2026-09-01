using Bogus;
using FCG.Catalog.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestReviewJsonBuilder
{
    public static RequestReviewJson Build()
    {
        return new Faker<RequestReviewJson>()
            .RuleFor(x => x.GameId, _ => Guid.NewGuid())
            .RuleFor(x => x.Rating, f => f.Random.Int(1, 5))
            .RuleFor(x => x.Comment, f => f.Lorem.Sentence())
            .RuleFor(x => x.Tags, f => f.Make(3, () => f.Lorem.Word()).Distinct().ToList());
    }
}

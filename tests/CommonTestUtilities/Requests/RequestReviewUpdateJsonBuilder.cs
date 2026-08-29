using Bogus;
using FCG.Catalog.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestReviewUpdateJsonBuilder
{
    public static RequestReviewUpdateJson Build()
    {
        return new Faker<RequestReviewUpdateJson>()
            .RuleFor(x => x.Rating, f => f.Random.Int(1, 5))
            .RuleFor(x => x.Comment, f => f.Lorem.Sentence())
            .RuleFor(x => x.Tags, f => f.Make(3, () => f.Lorem.Word()).Distinct().ToList());
    }
}

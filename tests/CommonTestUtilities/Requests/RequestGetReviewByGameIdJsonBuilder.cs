using Bogus;
using FCG.Catalog.Communication.Enums;
using FCG.Catalog.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestGetReviewByGameIdJsonBuilder
{
    public static RequestGetReviewByGameIdJson Build()
    {
        return new Faker<RequestGetReviewByGameIdJson>()
            .RuleFor(request => request.Page, f => f.Random.Int(1, 5))
            .RuleFor(request => request.PageSize, f => f.Random.Int(1, 10))
            .RuleFor(request => request.OrderBy, f => f.PickRandom<ReviewOrderBy>())
            .RuleFor(request => request.Desc, f => f.Random.Bool())
            .RuleFor(request => request.GameId, _ => Guid.NewGuid());
    }
}

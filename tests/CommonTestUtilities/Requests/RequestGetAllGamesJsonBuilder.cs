using Bogus;
using FCG.Catalog.Communication.Enums;
using FCG.Catalog.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestGetAllGamesJsonBuilder
{
    public static RequestGetAllGamesJson Build()
    {
        return new Faker<RequestGetAllGamesJson>()
            .RuleFor(request => request.Page, f => f.Random.Int(1, 5))
            .RuleFor(request => request.PageSize, f => f.Random.Int(1, 10))
            .RuleFor(request => request.OrderBy, f => f.PickRandom<GameOrderBy>())
            .RuleFor(request => request.Desc, f => f.Random.Bool())
            .RuleFor(request => request.Search, f => f.Commerce.ProductName());
    }
}

using Bogus;
using FCG.Catalog.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestSearchGamesJsonBuilder
{
    public static RequestSearchGamesJson Build()
    {
        return new Faker<RequestSearchGamesJson>()
            .RuleFor(request => request.Query, f => f.Commerce.ProductName())
            .RuleFor(request => request.Page, f => f.Random.Int(1, 5))
            .RuleFor(request => request.PageSize, f => f.Random.Int(1, 10));
    }
}

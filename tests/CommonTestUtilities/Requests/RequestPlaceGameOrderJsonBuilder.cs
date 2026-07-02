using Bogus;
using FCG.Catalog.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestPlaceGameOrderJsonBuilder
{
    public static RequestPlaceGameOrderJson Build()
    {
        return new Faker<RequestPlaceGameOrderJson>()
            .RuleFor(request => request.GameId, f => f.Random.Guid());
    }
}

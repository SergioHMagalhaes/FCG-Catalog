using Bogus;
using FCG.Catalog.Communication.Requests;
namespace CommonTestUtilities.Requests;

public class RequestGameJsonBuilder
{
    public static RequestGameJson Build()
    {
        return new Faker<RequestGameJson>()
            .RuleFor(x => x.Name, f => f.Commerce.ProductName())
            .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
            .RuleFor(x => x.Price, (f, x) => f.Random.Decimal(1, 500))
            .RuleFor(x => x.CategoryId, _ => 1);
    }
}

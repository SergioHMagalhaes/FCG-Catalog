using Bogus;
using FCG.Catalog.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestCategoryJsonBuilder
{
    public static RequestCategoryJson Build()
    {
        return new Faker<RequestCategoryJson>()
            .RuleFor(x => x.Name, f => f.Commerce.Categories(1).First());
    }
}

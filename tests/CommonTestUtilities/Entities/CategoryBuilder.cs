using Bogus;
using FCG.Catalog.Domain.Entities;
namespace CommonTestUtilities.Entities;

public class CategoryBuilder
{
    public static List<Category> Collection(uint count = 2)
    {
        var list = new List<Category>();

        if (count == 0)
            count = 1;

        var expenseId = 1;

        for(int i = 0; i < count; i++)
        {
            var category = Build();
            category.Id = expenseId++;

            list.Add(category);
        }

        return list;
    }

    public static Category Build()
    {
        return new Faker<Category>()
            .RuleFor(c => c.Id, _ => 1)
            .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0]);
    }
}

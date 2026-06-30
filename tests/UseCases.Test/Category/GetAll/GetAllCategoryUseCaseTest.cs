using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using FCG.Catalog.Application.UseCases.Category.GetAll;

namespace UseCases.Test.Category.GetAll;

public class GetAllCategoryUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var categories = CategoryBuilder.Collection(5);
        var useCase = CreateUseCase(categories);

        var result = await useCase.Execute();

        Assert.NotNull(result);
        Assert.Equal(categories.Count, result.Categories.Count);
    }

    private GetAllCategoryUseCase CreateUseCase(List<FCG.Catalog.Domain.Entities.Category> categories)
    {
        var repository = new CategoryRepositoryBuilder().GetAll(categories).Build();

        return new GetAllCategoryUseCase(repository);
    }
}

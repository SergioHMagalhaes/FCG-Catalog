using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using FCG.Catalog.Application.UseCases.Category.GetById;
using FCG.Catalog.Communication.Responses;

using FCG.Catalog.Exception.ExceptionsBase;

namespace UseCases.Test.Category.GetById;

public class GetCategoryByIdUseCaseTest
{

    [Fact]
    public async Task Success()
    {
        var category = CategoryBuilder.Build();
        var useCase = CreateUseCase(category);

        var result = await useCase.Execute(category.Id);

        Assert.NotNull(result);
        Assert.Equal(category.Id, result.Id);
        Assert.Equal(category.Name, result.Name);
    }

    [Fact]
    public async Task Error_Category_Not_Found()
    {
        var category = CategoryBuilder.Build();
        var useCase = CreateUseCase(category);

        async Task<ResponseCategoryJson> act() => await useCase.Execute(1000);

        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    private GetCategoryByIdUseCase CreateUseCase(FCG.Catalog.Domain.Entities.Category category)
    {
        var repository = new CategoryRepositoryBuilder().GetById(category).Build();

        return new GetCategoryByIdUseCase(repository);
    }
}

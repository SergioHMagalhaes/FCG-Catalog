using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using FCG.Catalog.Application.UseCases.Category.Delete;
using FCG.Catalog.Application.UseCases.Category.Update;
using FCG.Catalog.Exception.ExceptionsBase;

namespace UseCases.Test.Category.Delete;

public class DeleteCategoryUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var category = CategoryBuilder.Build();
        var useCase = CreateUseCase(category);

        await useCase.Execute(category.Id);
    }

    [Fact]
    public async Task Error_Category_Not_Found()
    {
        var useCase = CreateUseCase();

        async Task act() => await useCase.Execute(id: 1000);

        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    private DeleteCategoryUseCase CreateUseCase(FCG.Catalog.Domain.Entities.Category? category = null)
    {
        var repository = new CategoryRepositoryBuilder();
        var unitOfWork = new UnitOfWorkBuilder().Build();

        if(category is not null)
            repository.GetByIdTracked(category);
        else
            repository.GetByIdTrackedNotFound();

        return new DeleteCategoryUseCase(repository.Build(), unitOfWork);
    }
}

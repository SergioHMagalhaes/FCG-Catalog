using Bogus.DataSets;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FCG.Catalog.Application.UseCases.Category.GetById;
using FCG.Catalog.Application.UseCases.Category.Update;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Exception.ExceptionsBase;

namespace UseCases.Test.Category.Update;

public class UpdateCategoryUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var category = CategoryBuilder.Build();
        var useCase = CreateUseCase(category);
        var request = RequestCategoryJsonBuilder.Build();

        await useCase.Execute(category.Id, request);

        Assert.Equal(request.Name, category.Name);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var category = CategoryBuilder.Build();
        var request = RequestCategoryJsonBuilder.Build();
        request.Name = string.Empty;
        var useCase = CreateUseCase(category);

        async Task act() => await useCase.Execute(category.Id, request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
    }

    [Fact]
    public async Task Error_Name_Already_Exist()
    {
        var category = CategoryBuilder.Build();
        var request = RequestCategoryJsonBuilder.Build();
        var useCase = CreateUseCase(category, request.Name);

        async Task act() => await useCase.Execute(category.Id, request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
    }

    private UpdateCategoryUseCase CreateUseCase(FCG.Catalog.Domain.Entities.Category category, string? name = null)
    {
        var repository = new CategoryRepositoryBuilder().GetByIdTracked(category);
        var unitOfWork = new UnitOfWorkBuilder().Build();

        if (string.IsNullOrWhiteSpace(name) == false)
        {
            repository.ExistCategoryWithName(name);
        }

        return new UpdateCategoryUseCase(repository.Build(), unitOfWork);
    }
}

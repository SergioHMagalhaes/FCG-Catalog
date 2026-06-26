using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FCG.Catalog.Application.UseCases.Category.Register;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Exception.ExceptionsBase;

namespace UseCases.Test.Category.Register;

public class RegisterCategoryUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestCategoryJsonBuilder.Build();
        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        Assert.NotNull(result);
        Assert.Equal(request.Name, result.Name);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestCategoryJsonBuilder.Build();
        request.Name = string.Empty;
        var useCase = CreateUseCase();

        async Task<ResponseRegisterdCategoryJson> act() => await useCase.Execute(request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
    }

    [Fact]
    public async Task Error_Name_Already_Exist()
    {
        var request = RequestCategoryJsonBuilder.Build();
        var useCase = CreateUseCase(request.Name);

        async Task<ResponseRegisterdCategoryJson> act() => await useCase.Execute(request);

        await Assert.ThrowsAsync<ErrorOnValidationException>((Func<Task<ResponseRegisterdCategoryJson>>)act);
    }

    private RegisterCategoryUseCase CreateUseCase(string? name = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var repository = new CategoryRepositoryBuilder();

        if (string.IsNullOrWhiteSpace(name) == false)
        {
            repository.ExistCategoryWithName(name);
        }

        return new RegisterCategoryUseCase(repository.Build(), unitOfWork);
    }
}

using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FCG.Catalog.Application.UseCases.Game.Register;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Exception.ExceptionsBase;

namespace UseCases.Test.Game.Register;

internal class CreateUseCase {
    public required RegisterGameUseCase UseCase;
    public required UnitOfWorkBuilder UnitOfWork;
}

public class RegisterGameUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestGameJsonBuilder.Build();
        var useCase = CreateUseCase(request).UseCase;

        var result = await useCase.Execute(request);

        Assert.NotNull(result);
        Assert.Equal(request.Name, result.Name);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestGameJsonBuilder.Build();
        request.Name = string.Empty;
        var useCase = CreateUseCase().UseCase;

        async Task<ResponseRegisterdGameJson> act() => await useCase.Execute(request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
    }

    [Theory]
    [InlineData("   ")]
    [InlineData("")]
    public async Task Error_Name_WhiteSpace(string name)
    {
        var request = RequestGameJsonBuilder.Build();
        request.Name = name;
        var useCase = CreateUseCase().UseCase;

        async Task<ResponseRegisterdGameJson> act() => await useCase.Execute(request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
    }

    [Fact]
    public async Task Error_Description_Empty()
    {
        var request = RequestGameJsonBuilder.Build();
        request.Description = string.Empty;
        var useCase = CreateUseCase().UseCase;

        async Task<ResponseRegisterdGameJson> act() => await useCase.Execute(request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
    }

    [Theory]
    [InlineData("   ")]
    [InlineData("")]
    public async Task Error_Description_WhiteSpace(string description)
    {
        var request = RequestGameJsonBuilder.Build();
        request.Description = description;
        var useCase = CreateUseCase().UseCase;

        async Task<ResponseRegisterdGameJson> act() => await useCase.Execute(request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10.50)]
    public async Task Error_Price_Invalid(decimal price)
    {
        var request = RequestGameJsonBuilder.Build();
        request.Price = price;
        var useCase = CreateUseCase().UseCase;

        async Task<ResponseRegisterdGameJson> act() => await useCase.Execute(request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Error_CategoryId_Invalid(long categoryId)
    {
        var request = RequestGameJsonBuilder.Build();
        request.CategoryId = categoryId;
        var useCase = CreateUseCase().UseCase;

        async Task<ResponseRegisterdGameJson> act() => await useCase.Execute(request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
    }

    [Fact]
    public async Task Error_Category_Not_Found()
    {
        var request = RequestGameJsonBuilder.Build();
        request.CategoryId = 999;

        var useCase = CreateUseCase().UseCase;

        async Task<ResponseRegisterdGameJson> act() => await useCase.Execute(request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
    }

    [Fact]
    public async Task Success_Should_Call_Commit()
    {
        var request = RequestGameJsonBuilder.Build();
        var sut = CreateUseCase(request);
        var useCase = sut.UseCase;
        var unitOfWorkBuilder = sut.UnitOfWork;

        await useCase.Execute(request);

        unitOfWorkBuilder.VerifyCommitOnce();
    }

    [Fact]
    public async Task Error_Name_Empty_Should_Not_Call_Commit()
    {
        var request = RequestGameJsonBuilder.Build();
        request.Name = string.Empty;

        var sut = CreateUseCase();
        var unitOfWorkBuilder = sut.UnitOfWork;
        var useCase = sut.UseCase;

        async Task<ResponseRegisterdGameJson> act() => await useCase.Execute(request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);

        unitOfWorkBuilder.VerifyCommitNever();
    }

    private CreateUseCase CreateUseCase(RequestGameJson? request = null)
    {
        var unitOfWorkBuilder = new UnitOfWorkBuilder();
        var unitOfWork = unitOfWorkBuilder.Build();
        var repository = new GameRepositoryBuilder().Build();
        var categoryRepository = new CategoryRepositoryBuilder();

        if (request != null)
        {
            var categoryEntity = new FCG.Catalog.Domain.Entities.Category() { Id = request.CategoryId, Name = "test_value" };
            categoryRepository.GetById(categoryEntity);
        }

        return new CreateUseCase
        {
            UseCase = new RegisterGameUseCase(repository, categoryRepository.Build(), unitOfWork),
            UnitOfWork = unitOfWorkBuilder
        };
    }
}
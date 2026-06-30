using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FCG.Catalog.Application.UseCases.Game.Update;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Exception.ExceptionsBase;
using UseCases.Test.Game.Register;

namespace UseCases.Test.Game.Update;

internal class Sut
{
    public required UpdateGameUseCase UseCase;
    public required UnitOfWorkBuilder UnitOfWork;
}

public class UpdateGameUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var game = GameBuilder.Build();
        var request = RequestGameJsonBuilder.Build();
        var useCase = CreateSut(game, request).UseCase;

        await useCase.Execute(game.Id, request);

        Assert.Equal(request.Name, game.Name);
        Assert.Equal(request.Description, game.Description);
        Assert.Equal(request.Price, game.Price);
        Assert.Equal(request.CategoryId, game.CategoryId);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var game = GameBuilder.Build();
        var request = RequestGameJsonBuilder.Build();
        request.Name = string.Empty;
        var useCase = CreateSut(game).UseCase;

        async Task act() => await useCase.Execute(game.Id, request);
        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
    }

    [Theory]
    [InlineData("   ")]
    [InlineData("")]
    public async Task Error_Name_WhiteSpace(string name)
    {
        var game = GameBuilder.Build();
        var request = RequestGameJsonBuilder.Build();
        request.Name = name;
        var useCase = CreateSut(game).UseCase;

        async Task act() => await useCase.Execute(game.Id, request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
    }

    [Fact]
    public async Task Error_Description_Empty()
    {
        var game = GameBuilder.Build();
        var request = RequestGameJsonBuilder.Build();
        request.Description = string.Empty;
        var useCase = CreateSut(game).UseCase;

        async Task act() => await useCase.Execute(game.Id, request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
    }

    [Theory]
    [InlineData("   ")]
    [InlineData("")]
    public async Task Error_Description_WhiteSpace(string description)
    {
        var game = GameBuilder.Build();
        var request = RequestGameJsonBuilder.Build();
        request.Description = description;
        var useCase = CreateSut(game).UseCase;

        async Task act() => await useCase.Execute(game.Id, request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10.50)]
    public async Task Error_Price_Invalid(decimal price)
    {
        var game = GameBuilder.Build();
        var request = RequestGameJsonBuilder.Build();
        request.Price = price;
        var useCase = CreateSut(game).UseCase;

        async Task act() => await useCase.Execute(game.Id, request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Error_CategoryId_Invalid(long categoryId)
    {
        var game = GameBuilder.Build();
        var request = RequestGameJsonBuilder.Build();
        request.CategoryId = categoryId;
        var useCase = CreateSut(game).UseCase;

        async Task act() => await useCase.Execute(game.Id, request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
    }

    [Fact]
    public async Task Error_Category_Not_Found()
    {
        var game = GameBuilder.Build();
        var request = RequestGameJsonBuilder.Build();
        request.CategoryId = 999;

        var useCase = CreateSut(game).UseCase;

        async Task act() => await useCase.Execute(game.Id, request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);
    }

    [Fact]
    public async Task Success_Should_Call_Commit()
    {
        var game = GameBuilder.Build();
        var request = RequestGameJsonBuilder.Build();
        var sut = CreateSut(game, request);
        var useCase = sut.UseCase;
        var unitOfWorkBuilder = sut.UnitOfWork;

        await useCase.Execute(game.Id, request);
        unitOfWorkBuilder.VerifyCommitOnce();
    }

    [Fact]
    public async Task Error_Name_Empty_Should_Not_Call_Commit()
    {
        var game = GameBuilder.Build();
        var request = RequestGameJsonBuilder.Build();
        request.Name = string.Empty;

        var sut = CreateSut(game);
        var unitOfWorkBuilder = sut.UnitOfWork;
        var useCase = sut.UseCase;

        async Task act() => await useCase.Execute(game.Id, request);

        await Assert.ThrowsAsync<ErrorOnValidationException>(act);

        unitOfWorkBuilder.VerifyCommitNever();
    }


    private Sut CreateSut(FCG.Catalog.Domain.Entities.Game game, RequestGameJson? request = null)
    {
        var repository = new GameRepositoryBuilder().GetByIdTracked(game);
        var categoryRepository = new CategoryRepositoryBuilder();
        var unitOfWorkBuilder = new UnitOfWorkBuilder();
        var unitOfWork = unitOfWorkBuilder.Build();

        if (request != null)
        {
            var categoryEntity = new FCG.Catalog.Domain.Entities.Category() { Id = request.CategoryId, Name = "test_value" };
            categoryRepository.GetById(categoryEntity);
        }

        return new Sut
        {
            UseCase = new UpdateGameUseCase(repository.Build(), categoryRepository.Build(), unitOfWork),
            UnitOfWork = unitOfWorkBuilder
        };
    }
}

using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services;
using FCG.Catalog.Application.UseCases.Libraries.GetUserLibrary;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Exception.ExceptionsBase;

namespace UseCases.Test.Libraries.GetUserLibrary;

internal class Sut
{
    public required GetUserLibraryUseCase UseCase;
    public required LibraryRepositoryBuilder Repository;
    public required Guid UserId;
}

public class GetUserLibraryUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var userId = Guid.NewGuid();
        var library = LibraryBuilder.Build(userId, 5);
        var useCase = CreateSut(userId, library).UseCase;

        var result = await useCase.Execute();

        Assert.NotNull(result);
        Assert.Equal(library.Id, result.Id);
        Assert.Equal(library.ExternalId, result.ExternalId);
        Assert.Equal(library.UserId, result.UserId);
        Assert.Equal(library.Games.Count, result.Games.Count);
    }

    [Fact]
    public async Task Success_Should_Return_Library_Games()
    {
        var userId = Guid.NewGuid();
        var library = LibraryBuilder.Build(userId, 3);
        var useCase = CreateSut(userId, library).UseCase;

        var result = await useCase.Execute();

        Assert.Collection(result.Games,
            game =>
            {
                var expectedGame = library.Games.ElementAt(0);
                Assert.Equal(expectedGame.Id, game.Id);
                Assert.Equal(expectedGame.ExternalId, game.ExternalId);
                Assert.Equal(expectedGame.Name, game.Name);
                Assert.Equal(expectedGame.Price, game.Price);
            },
            game =>
            {
                var expectedGame = library.Games.ElementAt(1);
                Assert.Equal(expectedGame.Id, game.Id);
                Assert.Equal(expectedGame.ExternalId, game.ExternalId);
                Assert.Equal(expectedGame.Name, game.Name);
                Assert.Equal(expectedGame.Price, game.Price);
            },
            game =>
            {
                var expectedGame = library.Games.ElementAt(2);
                Assert.Equal(expectedGame.Id, game.Id);
                Assert.Equal(expectedGame.ExternalId, game.ExternalId);
                Assert.Equal(expectedGame.Name, game.Name);
                Assert.Equal(expectedGame.Price, game.Price);
            });
    }

    [Fact]
    public async Task Success_Should_Get_Library_By_Logged_User()
    {
        var userId = Guid.NewGuid();
        var library = LibraryBuilder.Build(userId);
        var sut = CreateSut(userId, library);

        await sut.UseCase.Execute();

        sut.Repository.VerifyGetByUserId(sut.UserId);
    }

    [Fact]
    public async Task Error_Library_Not_Found()
    {
        var userId = Guid.NewGuid();
        var useCase = CreateSut(userId, null).UseCase;

        async Task<ResponseUserLibraryJson> act() => await useCase.Execute();

        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    private Sut CreateSut(Guid userId, Library? library)
    {
        var repository = new LibraryRepositoryBuilder();
        repository.GetByUserId(userId, library);

        var loggedUser = LoggedUserBuilder.Build(userId);

        return new Sut
        {
            UseCase = new GetUserLibraryUseCase(loggedUser, repository.Build()),
            Repository = repository,
            UserId = userId
        };
    }
}

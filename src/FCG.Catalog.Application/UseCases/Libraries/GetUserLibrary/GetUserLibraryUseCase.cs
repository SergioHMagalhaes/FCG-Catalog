using FCG.Catalog.Application.Extensions;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Services.LoggedUser;
using FCG.Catalog.Exception.ExceptionsBase;

namespace FCG.Catalog.Application.UseCases.Libraries.GetUserLibrary;

public class GetUserLibraryUseCase : IGetUserLibraryUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly ILibraryRepository _repository;
    public GetUserLibraryUseCase(
        ILoggedUser loggedUser,
        ILibraryRepository repository)
    {
        _loggedUser = loggedUser;
        _repository = repository;
    }
    public async Task<ResponseUserLibraryJson> Execute()
    {
        var userId = _loggedUser.GetId();

        var result = await _repository.GetByUserId(userId);

        if (result == null)
            throw new NotFoundException("Biblioteca não encontrada");

        return result.MapToResponse();
    }
}

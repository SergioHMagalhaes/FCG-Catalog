using FCG.Catalog.Communication.Responses;

namespace FCG.Catalog.Application.UseCases.Libraries.GetUserLibrary;

public interface IGetUserLibraryUseCase
{
    Task<ResponseUserLibraryJson> Execute();
}

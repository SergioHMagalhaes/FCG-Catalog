using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;

namespace FCG.Catalog.Application.UseCases.Games.GetAll;

public interface IGetAllGamesUseCase
{
    Task<ResponseGamesJson> Execute(RequestGetAllGamesJson request);
}

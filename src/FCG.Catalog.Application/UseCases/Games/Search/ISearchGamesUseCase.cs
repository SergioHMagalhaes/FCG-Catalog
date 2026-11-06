using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;

namespace FCG.Catalog.Application.UseCases.Games.Search;

public interface ISearchGamesUseCase
{
    Task<ResponseGamesJson> Execute(RequestSearchGamesJson request);
}

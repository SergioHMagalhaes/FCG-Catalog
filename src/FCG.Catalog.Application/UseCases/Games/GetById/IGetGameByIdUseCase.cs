using FCG.Catalog.Communication.Responses;

namespace FCG.Catalog.Application.UseCases.Games.GetById;

public interface IGetGameByIdUseCase
{
    Task<ResponseGameJson> Execute(long id);
}

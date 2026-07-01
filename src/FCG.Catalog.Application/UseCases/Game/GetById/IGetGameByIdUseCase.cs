using FCG.Catalog.Communication.Responses;

namespace FCG.Catalog.Application.UseCases.Game.GetById;

public interface IGetGameByIdUseCase
{
    Task<ResponseGameJson> Execute(long id);
}

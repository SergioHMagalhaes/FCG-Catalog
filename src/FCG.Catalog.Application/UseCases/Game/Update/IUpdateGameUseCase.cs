using FCG.Catalog.Communication.Requests;

namespace FCG.Catalog.Application.UseCases.Game.Update;

public interface IUpdateGameUseCase
{
    Task Execute(long id, RequestGameJson request);
}

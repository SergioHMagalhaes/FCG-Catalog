using FCG.Catalog.Communication.Requests;

namespace FCG.Catalog.Application.UseCases.Games.Update;

public interface IUpdateGameUseCase
{
    Task Execute(long id, RequestGameJson request);
}

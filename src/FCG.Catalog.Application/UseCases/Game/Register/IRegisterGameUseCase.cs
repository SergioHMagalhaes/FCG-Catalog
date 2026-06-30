using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;

namespace FCG.Catalog.Application.UseCases.Game.Register;

public interface IRegisterGameUseCase
{
    Task<ResponseRegisterdGameJson> Execute(RequestGameJson request);
}

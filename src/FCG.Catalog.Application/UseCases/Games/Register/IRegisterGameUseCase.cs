using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;

namespace FCG.Catalog.Application.UseCases.Games.Register;

public interface IRegisterGameUseCase
{
    Task<ResponseRegisterdGameJson> Execute(RequestGameJson request);
}

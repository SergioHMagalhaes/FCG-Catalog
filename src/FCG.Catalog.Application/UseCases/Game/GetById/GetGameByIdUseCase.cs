using FCG.Catalog.Application.Extensions;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Exception.ExceptionsBase;

namespace FCG.Catalog.Application.UseCases.Game.GetById;

public class GetGameByIdUseCase : IGetGameByIdUseCase
{
    private readonly IGameRepository _repository;
    public GetGameByIdUseCase(IGameRepository repository)
    {
        _repository = repository;
    }
    public async Task<ResponseGameJson> Execute(long id)
    {
        var result = await _repository.GetById(id);

        if (result == null)
            throw new NotFoundException("Categoria não encontrada");

        return result.MapToResponse();
    }
}

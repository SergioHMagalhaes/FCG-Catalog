using FCG.Catalog.Application.Extensions;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Constants;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Services.Caching;
using FCG.Catalog.Exception.ExceptionsBase;

namespace FCG.Catalog.Application.UseCases.Games.GetById;

public class GetGameByIdUseCase : IGetGameByIdUseCase
{
    private readonly IGameRepository _repository;
    private readonly ICacheService _cacheService;

    public GetGameByIdUseCase(IGameRepository repository, ICacheService cacheService)
    {
        _repository = repository;
        _cacheService = cacheService;
    }

    public async Task<ResponseGameJson> Execute(long id)
    {
        var cacheKey = CacheKeys.Games.ById(id);

        var cachedResponse = await _cacheService.GetAsync<ResponseGameJson>(cacheKey);
        if (cachedResponse is not null)
        {
            return cachedResponse;
        }

        var result = await _repository.GetById(id);

        if (result == null)
            throw new NotFoundException("Jogo não encontrado");

        var response = result.MapToResponse();

        await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(30));

        return response;
    }
}


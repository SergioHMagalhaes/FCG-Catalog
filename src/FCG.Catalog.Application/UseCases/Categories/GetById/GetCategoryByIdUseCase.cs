using FCG.Catalog.Application.Extensions;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Constants;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Services.Caching;
using FCG.Catalog.Exception.ExceptionsBase;

namespace FCG.Catalog.Application.UseCases.Categories.GetById;

public class GetCategoryByIdUseCase : IGetCategoryByIdUseCase
{
    private readonly ICategoryRepository _repository;
    private readonly ICacheService _cacheService;

    public GetCategoryByIdUseCase(ICategoryRepository repository, ICacheService cacheService)
    {
        _repository = repository;
        _cacheService = cacheService;
    }

    public async Task<ResponseCategoryJson> Execute(long id)
    {
        var cacheKey = CacheKeys.Categories.ById(id);

        var cachedResponse = await _cacheService.GetAsync<ResponseCategoryJson>(cacheKey);
        if (cachedResponse is not null)
        {
            return cachedResponse;
        }

        var result = await _repository.GetById(id);

        if (result == null)
            throw new NotFoundException("Categoria não encontrada");

        var response = result.MapToResponse();

        await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(60));

        return response;
    }
}


using FCG.Catalog.Application.Extensions;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Constants;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Services.Caching;

namespace FCG.Catalog.Application.UseCases.Categories.GetAll;

public class GetAllCategoryUseCase : IGetAllCategoryUseCase
{
    private readonly ICategoryRepository _repository;
    private readonly ICacheService _cacheService;

    public GetAllCategoryUseCase(ICategoryRepository repository, ICacheService cacheService)
    {
        _repository = repository;
        _cacheService = cacheService;
    }

    public async Task<ResponseCategoriesJson> Execute()
    {
        var cacheKey = CacheKeys.Categories.All;

        var cachedResponse = await _cacheService.GetAsync<ResponseCategoriesJson>(cacheKey);
        if (cachedResponse is not null)
        {
            return cachedResponse;
        }

        var result = await _repository.GetAll();

        var response = new ResponseCategoriesJson
        {
            Categories = result.MapToResponse()
        };

        await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(60));

        return response;
    }
}


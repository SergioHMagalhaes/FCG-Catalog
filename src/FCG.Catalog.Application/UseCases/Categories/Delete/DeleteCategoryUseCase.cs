using FCG.Catalog.Domain.Constants;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Services.Caching;
using FCG.Catalog.Exception.ExceptionsBase;

namespace FCG.Catalog.Application.UseCases.Categories.Delete;

public class DeleteCategoryUseCase : IDeleteCategoryUseCase
{
    private readonly ICategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public DeleteCategoryUseCase(
        ICategoryRepository repository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }
    public async Task Execute(long id)
    {
        var category = await _repository.GetByIdTracked(id);

        if (category == null)
            throw new NotFoundException("Categoria não encontrada.");

        await _repository.Delete(id);
        await _unitOfWork.Commit();

        await _cacheService.RemoveAsync(CacheKeys.Categories.ById(id));
        await _cacheService.RemoveAsync(CacheKeys.Categories.All);
        await _cacheService.RemoveByPrefixAsync(CacheKeys.Games.ListPrefix);
    }
}

using FCG.Catalog.Domain.Constants;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Services.Caching;
using FCG.Catalog.Domain.Services.Search;
using FCG.Catalog.Exception.ExceptionsBase;

namespace FCG.Catalog.Application.UseCases.Games.Delete;

public class DeleteGameUseCase : IDeleteGameUseCase
{
    private readonly IGameRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly IGameSearchService _searchService;

    public DeleteGameUseCase(
        IGameRepository repository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        IGameSearchService searchService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _searchService = searchService;
    }

    public async Task Execute(long id)
    {
        var game = await _repository.GetByIdTracked(id);

        if (game == null)
            throw new NotFoundException("Jogo não encontrado.");

        await _repository.Delete(id);
        await _unitOfWork.Commit();

        await _cacheService.RemoveAsync(CacheKeys.Games.ById(id));
        await _cacheService.RemoveByPrefixAsync(CacheKeys.Games.ListPrefix);
        await _searchService.DeleteAsync(id);
    }
}

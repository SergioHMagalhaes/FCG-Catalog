using FCG.Catalog.Application.Extensions;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Domain.Constants;
using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Services.Caching;
using FCG.Catalog.Domain.Services.Search;
using FCG.Catalog.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace FCG.Catalog.Application.UseCases.Games.Update;

public class UpdateGameUseCase : IUpdateGameUseCase
{
    private readonly IGameRepository _repository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly IGameSearchService _searchService;

    public UpdateGameUseCase(
        IGameRepository repository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        IGameSearchService searchService)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _searchService = searchService;
    }
    public async Task Execute(long id, RequestGameJson request)
    {
        var category = await Validate(request);

        var game = await _repository.GetByIdTracked(id);

        if (game == null)
        {
            throw new NotFoundException("Jogo não encontrado.");
        }

        game = request.MapToDomain(game);
        game.Category = category!;

        _repository.Update(game);
        await _unitOfWork.Commit();

        await _cacheService.RemoveAsync(CacheKeys.Games.ById(id));
        await _cacheService.RemoveByPrefixAsync(CacheKeys.Games.ListPrefix);
        await _searchService.IndexAsync(game);
    }

    private async Task<Category?> Validate(RequestGameJson request)
    {
        var result = new RegisterGameValidator().Validate(request);

        var category = await _categoryRepository.GetByIdTracked(request.CategoryId);
        if (category is null)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, "Categoria não encontrada."));
        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }

        return category;
    }
}

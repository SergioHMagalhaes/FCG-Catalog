using FCG.Catalog.Application.Extensions;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Domain.Constants;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Services.Caching;
using FCG.Catalog.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace FCG.Catalog.Application.UseCases.Categories.Update;

public class UpdateCategoryUseCase : IUpdateCategoryUseCase
{
    private readonly ICategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public UpdateCategoryUseCase(
        ICategoryRepository repository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task Execute(long id, RequestCategoryJson request)
    {
        await Validate(request);

        var category = await _repository.GetByIdTracked(id);

        if (category is null)
        {
            throw new NotFoundException("Categoria não encontrada.");
        }

        _repository.Update(request.MapToDomain(category));
        await _unitOfWork.Commit();

        await _cacheService.RemoveAsync(CacheKeys.Categories.ById(id));
        await _cacheService.RemoveAsync(CacheKeys.Categories.All);
        await _cacheService.RemoveByPrefixAsync(CacheKeys.Games.ListPrefix);
    }

    private async Task Validate(RequestCategoryJson request)
    {
        var result = new RegisterCategoryValidator().Validate(request);

        var nameExists = await _repository.ExistsByName(request.Name);
        if (nameExists)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, "Categoria já cadastrada."));
        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}

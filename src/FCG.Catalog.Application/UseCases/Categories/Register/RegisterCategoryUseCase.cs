using FCG.Catalog.Application.Extensions;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Constants;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Services.Caching;
using FCG.Catalog.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace FCG.Catalog.Application.UseCases.Categories.Register;

public class RegisterCategoryUseCase : IRegisterCategoryUseCase
{
    private readonly ICategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public RegisterCategoryUseCase(
        ICategoryRepository repository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<ResponseRegisterdCategoryJson> Execute(RequestCategoryJson request)
    {
        await Validate(request);
        var category = request.MapToDomain();
        
        await _repository.Add(category);
        await _unitOfWork.Commit();

        await _cacheService.RemoveByPrefixAsync(CacheKeys.Categories.PrefixAll);

        return new ResponseRegisterdCategoryJson
        {
            Name = category.Name
        };
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

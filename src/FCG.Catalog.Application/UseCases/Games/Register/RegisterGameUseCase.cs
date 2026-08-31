using FCG.Catalog.Application.Extensions;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Constants;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Services.Caching;
using FCG.Catalog.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace FCG.Catalog.Application.UseCases.Games.Register;

public class RegisterGameUseCase : IRegisterGameUseCase
{
    private readonly IGameRepository _repository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public RegisterGameUseCase(
        IGameRepository repository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }
    public async Task<ResponseRegisterdGameJson> Execute(RequestGameJson request)
    {
        await Validate(request);
        var game = request.MapToDomain();
        
        await _repository.Add(game);
        await _unitOfWork.Commit();

        await _cacheService.RemoveByPrefixAsync(CacheKeys.Games.ListPrefix);

        return new ResponseRegisterdGameJson
        {
            Name = game.Name
        };
    }

    private async Task Validate(RequestGameJson request)
    {
        var result = new RegisterGameValidator().Validate(request);

        var nameExists = await _categoryRepository.GetById(request.CategoryId);
        if (nameExists is null)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, "Categoria não encontrada."));
        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}

using FCG.Catalog.Application.Extensions;
using FCG.Catalog.Application.UseCases.Category;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace FCG.Catalog.Application.UseCases.Game.Register;

public class RegisterGameUseCase : IRegisterGameUseCase
{
    private readonly IGameRepository _repository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterGameUseCase(
        IGameRepository repository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResponseRegisterdGameJson> Execute(RequestGameJson request)
    {
        await Validate(request);
        var game = request.MapToDomain();
        
        await _repository.Add(game);
        await _unitOfWork.Commit();

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

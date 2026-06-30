using FCG.Catalog.Application.Extensions;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace FCG.Catalog.Application.UseCases.Game.Update;

public class UpdateGameUseCase : IUpdateGameUseCase
{
    private readonly IGameRepository _repository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateGameUseCase(
        IGameRepository repository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task Execute(long id, RequestGameJson request)
    {
        await Validate(request);

        var game = await _repository.GetByIdTracked(id);

        if (game == null)
        {
            throw new NotFoundException("Jogo não encontrado.");
        }

        _repository.Update(request.MapToDomain(game));
        await _unitOfWork.Commit();
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

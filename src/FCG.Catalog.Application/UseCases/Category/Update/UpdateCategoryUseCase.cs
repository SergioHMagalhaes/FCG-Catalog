using FCG.Catalog.Application.Extensions;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace FCG.Catalog.Application.UseCases.Category.Update;

public class UpdateCategoryUseCase : IUpdateCategoryUseCase
{
    private readonly ICategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateCategoryUseCase(
        ICategoryRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
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

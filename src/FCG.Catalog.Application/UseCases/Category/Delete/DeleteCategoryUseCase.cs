using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Exception.ExceptionsBase;

namespace FCG.Catalog.Application.UseCases.Category.Delete;

public class DeleteCategoryUseCase : IDeleteCategoryUseCase
{
    private readonly ICategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteCategoryUseCase(
        ICategoryRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task Execute(long id)
    {
        var category = await _repository.GetByIdTracked(id);

        if (category == null)
            throw new NotFoundException("Categoria não encontrada.");

        await _repository.Delete(id);
        await _unitOfWork.Commit();
    }
}

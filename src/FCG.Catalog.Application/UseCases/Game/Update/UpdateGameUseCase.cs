using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Domain.Repositories;

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
    public Task Execute(long id, RequestGameJson request)
    {
        throw new NotImplementedException();
    }
}

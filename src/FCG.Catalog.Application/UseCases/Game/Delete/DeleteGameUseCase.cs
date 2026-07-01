using FCG.Catalog.Domain.Repositories;

namespace FCG.Catalog.Application.UseCases.Game.Delete;

public class DeleteGameUseCase : IDeleteGameUseCase
{
    private readonly IGameRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteGameUseCase(
        IGameRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public Task Execute(long id)
    {
        throw new NotImplementedException();
    }
}

using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Exception.ExceptionsBase;

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

    public async Task Execute(long id)
    {
        var game = await _repository.GetByIdTracked(id);

        if (game == null)
            throw new NotFoundException("Jogo não encontrado.");

        await _repository.Delete(id);
        await _unitOfWork.Commit();
    }
}

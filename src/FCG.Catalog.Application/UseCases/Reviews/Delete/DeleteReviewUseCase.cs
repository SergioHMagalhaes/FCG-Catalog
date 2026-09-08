using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Services.LoggedUser;
using FCG.Catalog.Exception.ExceptionsBase;

namespace FCG.Catalog.Application.UseCases.Reviews.Delete;

public class DeleteReviewUseCase : IDeleteReviewUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IReviewRepository _repository;
    public DeleteReviewUseCase(
        ILoggedUser loggedUser,
        IReviewRepository repository)
    {
        _loggedUser = loggedUser;
        _repository = repository;
    }
    public async Task Execute(Guid id)
    {
        var isAdmin = _loggedUser.IsAdmin();
        var userId = _loggedUser.GetId();

        var review = await _repository.GetById(id);

        if (review == null)
            throw new NotFoundException("Jogo não encontrado.");

        if(!isAdmin && review.UserId != userId)
            throw new UnauthorizedException("Você não tem permissão para deletar este review.");

        await _repository.Delete(id);
    }
}

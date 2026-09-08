using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Services.LoggedUser;
using FCG.Catalog.Exception.ExceptionsBase;

namespace FCG.Catalog.Application.UseCases.Reviews.MarkHelpfulVotes;

public class MarkHelpfulVotesUseCase : IMarkHelpfulVotesUseCase
{
    private readonly IReviewRepository _repository;
    public MarkHelpfulVotesUseCase(
        IReviewRepository repository)
    {
        _repository = repository;
    }
    public async Task Execute(Guid reviewId)
    {
        var review = await _repository.GetById(reviewId);
        if (review == null)
            throw new NotFoundException("Review não encontrada.");

        review.MarkAsHelpful();

        _repository.Update(review);
    }
}

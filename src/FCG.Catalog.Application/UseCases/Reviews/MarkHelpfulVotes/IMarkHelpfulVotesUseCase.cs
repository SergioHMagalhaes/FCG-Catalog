namespace FCG.Catalog.Application.UseCases.Reviews.MarkHelpfulVotes;

public interface IMarkHelpfulVotesUseCase
{
    Task Execute(Guid reviewId);
}

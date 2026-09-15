namespace FCG.Catalog.Application.UseCases.Reviews.Delete;

public interface IDeleteReviewUseCase
{
    Task Execute(Guid id);
}

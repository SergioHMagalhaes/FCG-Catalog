using FCG.Catalog.Communication.Requests;

namespace FCG.Catalog.Application.UseCases.Reviews.Update;

public interface IUpdateReviewUseCase
{
    Task Execute(Guid id, RequestReviewUpdateJson request);
}
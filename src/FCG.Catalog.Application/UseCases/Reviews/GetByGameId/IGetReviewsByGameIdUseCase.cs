using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;

namespace FCG.Catalog.Application.UseCases.Reviews.GetByGameId;

public interface IGetReviewsByGameIdUseCase
{
    Task<ResponseReviewsJson> Execute(RequestGetReviewByGameIdJson request);
}

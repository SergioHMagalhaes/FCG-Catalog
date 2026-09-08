using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;

namespace FCG.Catalog.Application.UseCases.Reviews.Register;

public interface IRegisterReviewUseCase
{
    Task<ResponseRegisterdReviewJson> Execute(RequestReviewJson request);
}

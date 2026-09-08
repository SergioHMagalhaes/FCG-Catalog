using FCG.Catalog.Application.Extensions;
using FCG.Catalog.Application.UseCases.Games.GetAll;
using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Exception.ExceptionsBase;

namespace FCG.Catalog.Application.UseCases.Reviews.GetByGameId;

public class GetReviewsByGameIdUseCase : IGetReviewsByGameIdUseCase
{
    private readonly IReviewRepository _repository;
    public GetReviewsByGameIdUseCase(
        IReviewRepository repository)
    {
        _repository = repository;
    }
    public async Task<ResponseReviewsJson> Execute(RequestGetReviewByGameIdJson request)
    {
        Validate(request);
        
        var filter = request.MapToDomain();
        var result = await _repository.GetByGameId(filter);

        return result.MapToResponse();
    }

    private void Validate(RequestGetReviewByGameIdJson request)
    {
        var validator = new GetReviewsByGameIdValidator();
        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessage = result.Errors.Select(f => f.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessage);
        }
    }
}

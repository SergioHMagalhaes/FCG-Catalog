using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Services.LoggedUser;
using FCG.Catalog.Exception.ExceptionsBase;

namespace FCG.Catalog.Application.UseCases.Reviews.Update;

public class UpdateReviewUseCase : IUpdateReviewUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IReviewRepository _repository;
    public UpdateReviewUseCase(
        ILoggedUser loggedUser,
        IReviewRepository repository)
    {
        _loggedUser = loggedUser;
        _repository = repository;
    }
    public async Task Execute(Guid id, RequestReviewUpdateJson request)
    {
        Validate(request);

        var userId = _loggedUser.GetId();

        var review = await _repository.GetById(id);

        if (review == null)
            throw new NotFoundException("Review não encontrada.");

        review.Update(userId, request.Rating, request.Comment, request.Tags);
        _repository.Update(review);
    }

    private void Validate(RequestReviewUpdateJson request)
    {
        var result = new UpdateReviewValidator().Validate(request);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}

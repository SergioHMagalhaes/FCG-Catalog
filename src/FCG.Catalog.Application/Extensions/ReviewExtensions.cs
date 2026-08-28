using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Entities;

namespace FCG.Catalog.Application.Extensions;

public static class ReviewExtensions
{
    public static ResponseRegisterdReviewJson MapToResponse(this Review review)
    {
        return new ResponseRegisterdReviewJson
        {
            Id = review.Id
        };
    }
}

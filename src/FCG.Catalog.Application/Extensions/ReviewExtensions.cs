using FCG.Catalog.Communication.Requests;
using FCG.Catalog.Communication.Responses;
using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Domain.Shared.Filters;
using FCG.Catalog.Domain.Shared.Pagination;

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

    public static ReviewFilter MapToDomain(this RequestGetReviewByGameIdJson request)
    {
        return new ReviewFilter
        {
            Page = request.Page,
            PageSize = request.PageSize,
            GameId = request.GameId,
            OrderBy = (Domain.Enums.ReviewOrderBy)request.OrderBy,
            Desc = request.Desc
        };
    }

    public static ResponseReviewsJson MapToResponse(this PagedResult<Review> pagedResult)
    {
        return new ResponseReviewsJson
        {
            Reviews = pagedResult.Items.Select(review => new ResponseReviewJson
            {
                Id = review.Id,
                GameId = review.GameId,
                UserId = review.UserId,
                UserName = review.UserName,
                Rating = review.Rating,
                Comment = review.Comment,
                HelpfulVotes = review.HelpfulVotes,
                Tags = review.Tags,
                CreatedAt = review.CreatedAt                
            }).ToList(),
            Page = pagedResult.Page,
            PageSize = pagedResult.PageSize,
            TotalCount = pagedResult.TotalCount,
            TotalPages = pagedResult.TotalPages,
            HasNextPage = pagedResult.HasNextPage,
            HasPreviousPage = pagedResult.HasPreviousPage
        };
    }
}

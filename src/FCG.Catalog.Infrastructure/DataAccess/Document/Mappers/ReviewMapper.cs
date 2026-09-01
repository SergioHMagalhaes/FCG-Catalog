using FCG.Catalog.Domain.Entities;
using FCG.Catalog.Infrastructure.DataAccess.Document.Documents;

namespace FCG.Catalog.Infrastructure.DataAccess.Document.Mappers;

public static class ReviewMapper
{
    public static ReviewDocument ToDocument(Review review) => new()
    {
        Id = review.Id,
        GameId = review.GameId,
        UserId = review.UserId,
        UserName = review.UserName,
        Rating = review.Rating,
        Comment = review.Comment,
        Tags = review.Tags.ToList(),
        CreatedAt = review.CreatedAt,
        HelpfulVotes = review.HelpfulVotes
    };

    public static Review ToDomain(ReviewDocument doc) =>
        Review.Rehydrate(
            doc.Id,
            doc.GameId,
            doc.UserId,
            doc.UserName,
            doc.Rating,
            doc.Comment,
            doc.Tags,
            doc.CreatedAt,
            doc.HelpfulVotes);
}